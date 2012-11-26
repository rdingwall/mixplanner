using System;
using System.Globalization;

namespace MixPlanner.CommandLine.DomainModel
{
    public class Key : IEquatable<Key>
    {
        // e.g. 12A
        private readonly ushort hexValue;

        public Key(int pitch, Scale scale) : this (String.Format("{0}{1:X}", pitch, (int)scale))
        {
        }

        public Key(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            hexValue = ushort.Parse(key, NumberStyles.HexNumber);

            Scale = ExtractScale(hexValue);
            Pitch = ExtractPitch(hexValue);
        }

        protected Key(ushort hexValue, Scale scale, int pitch)
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

        public bool HasSamePitchAs(Key other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return Pitch.Equals(other.Pitch);
        }

        public bool HasSameScaleAs(Key other)
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

        public static Key Key1A { get { return new Key("1A"); } }
        public static Key Key2A { get { return new Key("2A"); } }
        public static Key Key3A { get { return new Key("3A"); } }
        public static Key Key4A { get { return new Key("4A"); } }
        public static Key Key5A { get { return new Key("5A"); } }
        public static Key Key6A { get { return new Key("6A"); } }
        public static Key Key7A { get { return new Key("7A"); } }
        public static Key Key8A { get { return new Key("8A"); } }
        public static Key Key9A { get { return new Key("9A"); } }
        public static Key Key10A { get { return new Key("10A"); } }
        public static Key Key11A { get { return new Key("11A"); } }
        public static Key Key12A { get { return new Key("12A"); } }

        public static Key Key1B { get { return new Key("1B"); } }
        public static Key Key2B { get { return new Key("2B"); } }
        public static Key Key3B { get { return new Key("3B"); } }
        public static Key Key4B { get { return new Key("4B"); } }
        public static Key Key5B { get { return new Key("5B"); } }
        public static Key Key6B { get { return new Key("6B"); } }
        public static Key Key7B { get { return new Key("7B"); } }
        public static Key Key8B { get { return new Key("8B"); } }
        public static Key Key9B { get { return new Key("9B"); } }
        public static Key Key10B { get { return new Key("10B"); } }
        public static Key Key11B { get { return new Key("11B"); } }
        public static Key Key12B { get { return new Key("12B"); } }

        public static Key Unknown { get { return new UnknownKey();} }

        public static Key RandomKey()
        {
            var random = new Random();
            var pitch = random.Next(1, 12);
            var scale = random.Next(0, 1) == 1 ? Scale.Major : Scale.Minor;
            return new Key(pitch, scale);
        }

        public bool Equals(Key other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Scale, Scale) && other.Pitch == Pitch;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Key)) return false;
            return Equals((Key) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Scale.GetHashCode()*397) ^ Pitch;
            }
        }

        public Key IncreasePitch(int value)
        {
            var newPitch = (Pitch + value) % 12;

            if (newPitch == 0)
                newPitch = 12;

            return new Key(newPitch, Scale);
        }

        public Key ToMinor()
        {
            return new Key(Pitch, Scale.Minor);
        }

        public Key ToMajor()
        {
            return new Key(Pitch, Scale.Major);
        }

        public static bool TryParse(string str, out Key key)
        {
            try
            {
                key = new Key(str);
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