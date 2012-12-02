﻿using System;
using System.Globalization;

namespace MixPlanner.DomainModel
{
    public class HarmonicKey : IEquatable<HarmonicKey>
    {
        // e.g. 12A
        private readonly ushort hexValue;

        public HarmonicKey(int pitch, Scale scale) : this (String.Format("{0}{1:X}", pitch, (int)scale))
        {
        }

        public HarmonicKey(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            hexValue = ushort.Parse(key, NumberStyles.HexNumber);

            Scale = ExtractScale(hexValue);
            Pitch = ExtractPitch(hexValue);
        }

        protected HarmonicKey(ushort hexValue, Scale scale, int pitch)
        {
            this.hexValue = hexValue;
            Scale = scale;
            Pitch = pitch;
        }

        static int ExtractPitch(ushort key)
        {
            // Isolate the pitch e.g. 12A -> 12
            var hexPitchAsDec = key >> 4;
            var pitch = Convert.ToInt32(hexPitchAsDec.ToString("X"));

            ThrowIfInvalidPitch(pitch);

            return pitch;
        }

        static void ThrowIfInvalidPitch(int pitch)
        {
            if (!(pitch <= 12))
                throw new InvalidPitchException(String.Format("{0} is not a valid pitch (must be 0-12).", pitch));
        }

        static Scale ExtractScale(ushort key)
        {
            // Isolate the scale e.g. 12B -> B
            var scale = key & 0xF;

            ThrowIfInvalidScale(scale);

            return (Scale)scale;
        }

        static void ThrowIfInvalidScale(int scale)
        {
            if (!(scale == (ushort)Scale.Major || scale == (ushort)Scale.Minor))
                throw new InvalidScaleException("Invalid scale (must be A or B).");
        }

        public bool HasSamePitchAs(HarmonicKey other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return Pitch.Equals(other.Pitch);
        }

        public bool HasSameScaleAs(HarmonicKey other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return Scale.Equals(other.Scale);
        }

        public bool IsMajor()
        {
            return Scale == Scale.Major;
        }

        public bool IsMinor()
        {
            return Scale == Scale.Minor;
        }

        public Scale Scale { get; private set; }
        public int Pitch { get; private set; }

        public override string ToString()
        {
            return hexValue.ToString("X");
        }

        public static HarmonicKey Key1A { get { return new HarmonicKey("1A"); } }
        public static HarmonicKey Key2A { get { return new HarmonicKey("2A"); } }
        public static HarmonicKey Key3A { get { return new HarmonicKey("3A"); } }
        public static HarmonicKey Key4A { get { return new HarmonicKey("4A"); } }
        public static HarmonicKey Key5A { get { return new HarmonicKey("5A"); } }
        public static HarmonicKey Key6A { get { return new HarmonicKey("6A"); } }
        public static HarmonicKey Key7A { get { return new HarmonicKey("7A"); } }
        public static HarmonicKey Key8A { get { return new HarmonicKey("8A"); } }
        public static HarmonicKey Key9A { get { return new HarmonicKey("9A"); } }
        public static HarmonicKey Key10A { get { return new HarmonicKey("10A"); } }
        public static HarmonicKey Key11A { get { return new HarmonicKey("11A"); } }
        public static HarmonicKey Key12A { get { return new HarmonicKey("12A"); } }

        public static HarmonicKey Key1B { get { return new HarmonicKey("1B"); } }
        public static HarmonicKey Key2B { get { return new HarmonicKey("2B"); } }
        public static HarmonicKey Key3B { get { return new HarmonicKey("3B"); } }
        public static HarmonicKey Key4B { get { return new HarmonicKey("4B"); } }
        public static HarmonicKey Key5B { get { return new HarmonicKey("5B"); } }
        public static HarmonicKey Key6B { get { return new HarmonicKey("6B"); } }
        public static HarmonicKey Key7B { get { return new HarmonicKey("7B"); } }
        public static HarmonicKey Key8B { get { return new HarmonicKey("8B"); } }
        public static HarmonicKey Key9B { get { return new HarmonicKey("9B"); } }
        public static HarmonicKey Key10B { get { return new HarmonicKey("10B"); } }
        public static HarmonicKey Key11B { get { return new HarmonicKey("11B"); } }
        public static HarmonicKey Key12B { get { return new HarmonicKey("12B"); } }

        public static HarmonicKey Unknown { get { return new UnknownKey();} }

        public static HarmonicKey RandomKey()
        {
            var random = new Random();
            var pitch = random.Next(1, 12);
            var scale = random.Next(0, 1) == 1 ? Scale.Major : Scale.Minor;
            return new HarmonicKey(pitch, scale);
        }

        public bool Equals(HarmonicKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Scale, Scale) && other.Pitch == Pitch;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (HarmonicKey)) return false;
            return Equals((HarmonicKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Scale.GetHashCode()*397) ^ Pitch;
            }
        }

        public HarmonicKey IncreasePitch(int value)
        {
            var newPitch = (Pitch + value) % 12;

            if (newPitch == 0)
                newPitch = 12;

            return new HarmonicKey(newPitch, Scale);
        }

        public HarmonicKey ToMinor()
        {
            return new HarmonicKey(Pitch, Scale.Minor);
        }

        public HarmonicKey ToMajor()
        {
            return new HarmonicKey(Pitch, Scale.Major);
        }

        public static bool TryParse(string str, out HarmonicKey key)
        {
            try
            {
                key = new HarmonicKey(str);
                return true;
            }
            catch
            {
                key = null;
                return false;
            }
        }
    }
}