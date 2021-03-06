﻿namespace OpenKh.Kh2.Messages
{
    public enum MessageCommand
    {
        End,
        PrintText,
        PrintComplex,
        Tabulation,
        NewLine,
        Reset,
        Theme,
        Unknown05,
        Unknown06,
        Color,
        Unknown08,
        PrintIcon,
        TextScale, // Default: 0x10
        TextWidth, // Default: 0x48
        LineSpacing,
        Unknown0d,
        Unknown0e,
        Unknown0f,
        Clear,
        Position,
        Unknown12,
        Unknown13,
        Delay,
        CharDelay,
        Unknown16,
        DelayAndFade,
        Unknown18,
        Unknown19,
        Unknown1a,
        Unknown1b,
        Unknown1c,
        Unknown1d,
        Unknown1e,
        Unknown1f,
        Unsupported,
    }
}
