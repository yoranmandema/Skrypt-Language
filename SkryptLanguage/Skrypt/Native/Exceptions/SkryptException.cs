﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class SkryptException : Exception {
        public Error Error { get; }

        public override string Source => Error?.Source;
        public string Description => Error?.Message;
        public string File => Error?.File;
        public int Index => Error?.Index ?? -1;
        public int Line => Error?.Line ?? 0;
        public int Column => Error?.Column ?? 0;

        public SkryptException() :
            this(null, null, null) { }

        public SkryptException(string message) :
            this(message, null, null) { }

        public SkryptException(string message, Exception innerException) :
            this(message, null, innerException) { }

        public SkryptException(Error error) :
            this(null, error) { }

        public SkryptException(string message, Error error) :
            this(message, error, null) { }

        public SkryptException(string message, Error error, Exception innerException) : base(message ?? error?.ToString(), innerException) {
            Error = error;
        }
    }
}
