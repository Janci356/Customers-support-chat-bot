using Catalyst;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers_support_chat_bot
{
    internal class NLPModel
    {
        public NLPModel() 
        {
            Catalyst.Models.English.Register();
        }

        public async Task<IDocument> Process(string input) 
        {
            Pipeline model = await Pipeline.ForAsync(Language.English);
            Document document = new Document(input, Language.English);

            IDocument processedDoc = model.ProcessSingle(document);
            return processedDoc;
        }
    }
}
