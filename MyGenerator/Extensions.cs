namespace System.Text
{
    public static class Extensions
    {
        /// <inheritdoc cref="StringBuilder.AppendLine(string)"/>
        /// <remarks>Indents the provided value by the specified number of four space indents.</remarks>
        public static StringBuilder AppendLineIndented(this StringBuilder source, int numIndents, string value)
        {
            const string singleIndent = "    ";

            var indent = new StringBuilder(singleIndent.Length * numIndents).Insert(0, singleIndent, numIndents).ToString();

            return source.AppendLine(indent + value);
        }
    }
}