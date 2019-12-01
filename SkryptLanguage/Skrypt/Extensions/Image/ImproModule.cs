using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Extensions.Image {
    public class ImproModule : SkryptModule {
        public ImproModule(SkryptEngine engine) : base(engine) { }

        public static SkryptObject WriteImage(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var image = arguments.GetAs<ImageInstance>(0);
            var file = arguments.GetAs<StringInstance>(1);

            var destination = Path.Combine(engine.FileHandler.Folder, file);

            using (MemoryStream memory = new MemoryStream()) {
                using (FileStream fs = new FileStream(destination, FileMode.Create, FileAccess.ReadWrite)) {
                    image.bitMap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return null;
        }

        public class ImageType : SkryptType {
            public ImageType(SkryptEngine engine) : base(engine) {
                Template = engine.TemplateMaker.CreateTemplate(typeof(ImageInstance));
            }

            public SkryptInstance Construct(int width, int height) {
                var obj = new ImageInstance(Engine, width, height);

                obj.GetProperties(Template);
                obj.TypeObject = this;

                return obj;
            }

            public override SkryptInstance Construct(Arguments arguments) {
                return Construct(
                    (int)arguments.GetAs<NumberInstance>(0),
                    (int)arguments.GetAs<NumberInstance>(1)
                    );
            }
        }

        public class ImageInstance : SkryptInstance {
            public override string Name => "Image";

            public Bitmap bitMap;

            public ImageInstance(SkryptEngine engine) : base(engine) { }

            public ImageInstance(SkryptEngine engine, int width, int height) : base(engine) {
                bitMap = new Bitmap(width, height);

                CreateProperty("Width", engine.CreateNumber(width));
                CreateProperty("Height", engine.CreateNumber(height));
            }

            public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height) {
                Bitmap result = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(result)) {
                    g.DrawImage(bmp, 0, 0, width, height);
                }

                return result;
            }

            public static SkryptObject SetResolution(SkryptEngine engine, SkryptObject self, Arguments arguments) {
                var width = arguments.GetAs<NumberInstance>(0);
                var height = arguments.GetAs<NumberInstance>(1);

                self.AsType<ImageInstance>().bitMap.SetResolution((int)width, (int)height);

                self.SetProperty("Width", width);
                self.SetProperty("Height", height);

                return null;
            }

            public static SkryptObject Resize(SkryptEngine engine, SkryptObject self, Arguments arguments) {
                var width = arguments.GetAs<NumberInstance>(0);
                var height = arguments.GetAs<NumberInstance>(1);

                var newBitMap = ResizeBitmap(self.AsType<ImageInstance>().bitMap, (int)width, (int)height);

                self.AsType<ImageInstance>().bitMap = newBitMap;

                self.SetProperty("Width", width);
                self.SetProperty("Height", height);

                return null;
            }

            public static SkryptObject SetPixel(SkryptEngine engine, SkryptObject self, Arguments arguments) {
                var x = arguments.GetAs<NumberInstance>(0);
                var y = arguments.GetAs<NumberInstance>(1);
                var color = arguments.GetAs<ColorInstance>(2);

                self.AsType<ImageInstance>().bitMap.SetPixel((int)x, (int)y, color.color);

                return null;
            }

            public static SkryptObject GetPixel(SkryptEngine engine, SkryptObject self, Arguments arguments) {
                var x = arguments.GetAs<NumberInstance>(0);
                var y = arguments.GetAs<NumberInstance>(1);

                var color = self.AsType<ImageInstance>().bitMap.GetPixel((int)x, (int)y);

                return engine.GetValue("Color").AsType<ColorType>().Construct(color.R, color.G, color.B);
            }

            public override string ToString() {
                return $"Image({bitMap.Width},{bitMap.Height})";
            }
        }

        public class ColorType : SkryptType {
            public ColorType(SkryptEngine engine) : base(engine) {
                Template = engine.TemplateMaker.CreateTemplate(typeof(ColorInstance));
            }

            public SkryptInstance Construct(int r, int g, int b) {
                var obj = new ColorInstance(Engine, Color.FromArgb(255, r, g, b));

                obj.GetProperties(Template);
                obj.TypeObject = this;

                return obj;
            }

            public override SkryptInstance Construct(Arguments arguments) {
                int argCount = arguments.Length;

                if (argCount == 3) {
                    return Construct(
                        (int)arguments.GetAs<NumberInstance>(0),
                        (int)arguments.GetAs<NumberInstance>(1),
                        (int)arguments.GetAs<NumberInstance>(2)
                        );
                }
                else if (argCount == 1) {
                    var val = (int)arguments.GetAs<NumberInstance>(0);

                    return Construct(val, val, val);
                }
                else {
                    throw new ArgumentException("Expected at least 1 number input.");
                }
            }
        }

        public class ColorInstance : SkryptInstance {
            public override string Name => "Color";

            public Color color;

            public ColorInstance(SkryptEngine engine) : base(engine) { }

            public ColorInstance(SkryptEngine engine, Color col) : base(engine) {
                color = col;
            }

            public override string ToString() {
                return $"{color.R} {color.R} {color.B}";
            }
        }
    }
}
