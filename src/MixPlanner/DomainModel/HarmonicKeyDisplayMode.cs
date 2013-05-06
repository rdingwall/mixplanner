namespace MixPlanner.DomainModel
{
    public enum HarmonicKeyDisplayMode
    {
        KeyCode = 0, // default e.g. 2A
        TraditionalWithSymbols, // e.g. E♭
        TraditionalWithText, // e.g. E-Flat,
        Id3Tkey, // e.g. Ebm
        OpenKeyNotation, // e.g. 7m,
        Beatport // e.g. D#min
    }
}