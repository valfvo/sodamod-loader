namespace SodaMod.UI
{
    public class Element
    {
        public StyleDeclaration Style { get; private set; }

        public string Id;

        public Element appendChild(Element child)
        {
            return child;
        }
    }
}
