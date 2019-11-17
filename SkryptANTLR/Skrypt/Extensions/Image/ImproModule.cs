using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Extensions.Image {
    public class ImproModule : BaseModule {
        public ImproModule(Engine engine) : base(engine) { }

        public static BaseObject WriteImage(Engine engine, BaseObject self, Arguments arguments) {
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

        public class ImageType : BaseType {
            public ImageType(Engine engine) : base(engine) {
                Template = engine.TemplateMaker.CreateTemplate(typeof(ImageInstance));
            }

            public BaseInstance Construct(int width, int height) {
                var obj = new ImageInstance(Engine, width, height);

                obj.GetProperties(Template);
                obj.TypeObject = this;

                return obj;
            }

            public override BaseInstance Construct(Arguments arguments) {
                return Construct(
                    (int)arguments.GetAs<NumberInstance>(0),
                    (int)arguments.GetAs<NumberInstance>(1)
                    );
            }
        }

        public class ImageInstance : BaseInstance {
            public override string Name => "Image";

            public Bitmap bitMap;
            public ImageInstance(Engine engine) : base(engine) {}

            public ImageInstance(Engine engine, int width, int height) : base(engine) {
                bitMap = new Bitmap(width, height);

                CreateProperty("Width", engine.CreateNumber(width));
                CreateProperty("Height", engine.CreateNumber(height));
            }

            public static BaseObject SetPixel (Engine engine, BaseObject self, Arguments arguments) {
                var x = arguments.GetAs<NumberInstance>(0);
                var y = arguments.GetAs<NumberInstance>(1);
                var color = arguments.GetAs<ColorInstance>(2);

                ((ImageInstance)self).bitMap.SetPixel((int)x, (int)y, color.color);

                return null;
            }

            public static BaseObject GetPixel(Engine engine, BaseObject self, Arguments arguments) {
                var x = arguments.GetAs<NumberInstance>(0);
                var y = arguments.GetAs<NumberInstance>(1);

                var color = ((ImageInstance)self).bitMap.GetPixel((int)x, (int)y);

                return engine.GetValue("Color").AsType<ColorType>().Construct(color.R,color.G,color.B);
            }

            public override string ToString() {
                return $"Image({bitMap.Width},{bitMap.Height})";
            }
        }

        public class ColorType : BaseType {
            public ColorType(Engine engine) : base(engine) {
                Template = engine.TemplateMaker.CreateTemplate(typeof(ColorInstance));
            }

            public BaseInstance Construct(int r, int g, int b) {
                var obj = new ColorInstance(Engine, Color.FromArgb(255,r,g,b));

                obj.GetProperties(Template);
                obj.TypeObject = this;

                return obj;
            }

            public override BaseInstance Construct(Arguments arguments) {
                int argCount = arguments.Length;

                if (argCount == 3) {
                    return Construct(
                        (int)arguments.GetAs<NumberInstance>(0),
                        (int)arguments.GetAs<NumberInstance>(1),
                        (int)arguments.GetAs<NumberInstance>(2)
                        );
                } else if (argCount == 1) {
                    var val = (int)arguments.GetAs<NumberInstance>(0);

                    return Construct(val, val, val);
                } else {
                    throw new ArgumentException("Expected at least 1 number input.");
                }
            }
        }

        public class ColorInstance : BaseInstance {
            public override string Name => "Color";

            public Color color;
            public ColorInstance(Engine engine) : base(engine) { }
            public ColorInstance(Engine engine, Color col) : base(engine) {
                color = col;
            }
            public override string ToString() {
                return $"{color.R} {color.R} {color.B}";
            }
        }
    }
}
