namespace Oneill
{
    public enum Neighbour
    {
        None        = 0x00,
        All         = 0x3F,
        Any         = 0x40,

        UpDown      = 0x03,
        LeftRight   = 0x0C,
        FrontBack   = 0x30,

        Up          = 0x01,
        Down        = 0x02,
        Left        = 0x04,
        Right       = 0x08,
        Front       = 0x10,
        Back        = 0x20,
    }
}