using System.Collections.Generic;
using System.Linq;
using Results;

namespace SyntaxTextParser.Architecture
{
    public abstract class ElementParserWithRules : BaseElementParser
    {
        protected readonly IEnumerable<IElementValidator> ElementValidators;

        protected ElementParserWithRules(IEnumerable<IElementValidator> elementValidators, IElementFormatter elementFormatter) : 
            base(elementFormatter)
        {
            ElementValidators = elementValidators;
        }

        protected bool IsCorrectElement(TypedTextElement element)
        {
            return element != null && ElementValidators.ToList()
                .TrueForAll(x => x.IsValidElement(element));
        }

        public override Result<List<TextElement>> ParseElementsFromText(string text)
        {
            var textElements = new Dictionary<TypedTextElement, int>();
            var elements = Result.Of(() => ParseText(text).ToList());

            if (!elements.IsSuccess)
                return Result.Fail<List<TextElement>>(elements.Error);

            foreach (var element in elements.GetValueOrThrow())
            {
                if(!IsCorrectElement(element)) continue;

                if (textElements.ContainsKey(element))
                    textElements[element]++;
                else
                    textElements.Add(element, 1);
            }

            return textElements
                .Select(x => x.Key.ConvertToTextElement(x.Value))
                .ToList();
        }

        protected abstract IEnumerable<TypedTextElement> ParseText(string text);
    }
}