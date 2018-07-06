using System.Collections.Generic;

namespace Yol.Punla.FakeData
{
    public static class FakeWikis
    {
        private static IEnumerable<Entity.Wiki> WikisField;
        public static IEnumerable<Entity.Wiki> Wikis { get => WikisField; set => WikisField = value; }

        public static void Init()
        {
            WikisField = new List<Entity.Wiki>
            {
               new Entity.Wiki{ Title="Depression in Philippines", IconPath="icon.png", Content = @"<html><body style='background-color:#F5F5F5'><h3>Depression in Philippines</h3><p>We are top 1 in Asia in terms of depression</p></body></html>" },
               new Entity.Wiki{ Title="Mental health law to help fight illegal drugs", IconPath="icon.png", Content=@"<html><body style='background-color:#F5F5F5'><h3>Mental health law to help fight illegal drugs</h3><p>Senator Risa Hontiveros believes passing a Mental Health Law would also help the country fight illegal drugs.</p><p>Hontiveros said illegal drugs, which also deals with addiction, is not only a law enforcement issue but also a public health issue. &quot;<i>We should look at it also as a public health problem, hindi lang law enforcement. At sa ilalim niyan, mental health problem nga,&quot;</i> she told ANC on Monday.</p><p>The senator said the <i>Mental Health Bill</i>, which was recently passed on third reading at the Senate, will help patients become fully-functioning members of the society again. According to Hontiveros, mental health remains a concern as data show 7 Filipinos commit suicide everyday. She added that people with mental health concerns also suffer stigma from society.</p><p>Under the <i>Philippine Mental Health Act of 2017</i>, she said, mental health will be integrated into the school curriculum to reduce discrimination of patients. Hontiveros added that the Department of Health is mandated to increase mental health professionals in the country under the proposed measure.<i>&quot;May isang pag-aaral na half of all adult Filipinos consulting health professionals in rural areas, kalahati, may nade-detect na mental health problem dahil sa kakulangan ng pagkakataon para magpa-checkup,&quot;</i> she said.</p><p>Hontiveros hopes the House of Representatives will be able to pass its version of the Mental Health Bill this year.</p></body></html>" },
               new Entity.Wiki{ Title="Item 3" },
               new Entity.Wiki{ Title="Item 4" },
               new Entity.Wiki{ Title="Item 5" },
               new Entity.Wiki{ Title="Item 6" },
               new Entity.Wiki{ Title="Item 7" },
               new Entity.Wiki{ Title="Item 8" },
               new Entity.Wiki{ Title="Item 9" },
               new Entity.Wiki{ Title="Item 10" },
               new Entity.Wiki{ Title="Item 11" },
               new Entity.Wiki{ Title="Item 12" },
               new Entity.Wiki{ Title="Item 13" },
               new Entity.Wiki{ Title="Item 14" },
               new Entity.Wiki{ Title="Item 15" },
               new Entity.Wiki{ Title="Item 16" },
               new Entity.Wiki{ Title="Item 17" },
               new Entity.Wiki{ Title="Item 18" },
               new Entity.Wiki{ Title="Item 19" },
               new Entity.Wiki{ Title="Item 20" },
               new Entity.Wiki{ Title="Item 21" },
               new Entity.Wiki{ Title="1 out of 5 Filipinos suffer from depression", ForceToVersionNo = "1.9" }
            };
        }
    }
}
