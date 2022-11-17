namespace Proof.Core.Text
{
    public class FontCharacter
    {
        public FontCharacter(int id, int x, int y, int width, int height, int xOffset, int yOffset, int xAdvance)
        {
            Id = id;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;
            XAdvance = xAdvance;
        }

        public int Id { get; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public int XOffset { get; }
        public int YOffset { get; }
        public int XAdvance { get; }

    }
}
