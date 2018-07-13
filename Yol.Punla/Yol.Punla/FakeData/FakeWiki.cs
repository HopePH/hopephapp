using System.Collections.Generic;

namespace Yol.Punla.FakeData
{
    public static class FakeWikis
    {
        private static string WIKICONTENTS = @"<html><body style='background-color:#F5F5F5'><h1>{0}</h1><h3>Mental health law to help fight illegal drugs</h3><p>Senator Risa Hontiveros believes passing a Mental Health Law would also help the country fight illegal drugs.</p><p>Hontiveros said illegal drugs, which also deals with addiction, is not only a law enforcement issue but also a public health issue. &quot;<i>We should look at it also as a public health problem, hindi lang law enforcement. At sa ilalim niyan, mental health problem nga,&quot;</i> she told ANC on Monday.</p><p>The senator said the <i>Mental Health Bill</i>, which was recently passed on third reading at the Senate, will help patients become fully-functioning members of the society again. According to Hontiveros, mental health remains a concern as data show 7 Filipinos commit suicide everyday. She added that people with mental health concerns also suffer stigma from society.</p><p>Under the <i>Philippine Mental Health Act of 2017</i>, she said, mental health will be integrated into the school curriculum to reduce discrimination of patients. Hontiveros added that the Department of Health is mandated to increase mental health professionals in the country under the proposed measure.<i>&quot;May isang pag-aaral na half of all adult Filipinos consulting health professionals in rural areas, kalahati, may nade-detect na mental health problem dahil sa kakulangan ng pagkakataon para magpa-checkup,&quot;</i> she said.</p><p>Hontiveros hopes the House of Representatives will be able to pass its version of the Mental Health Bill this year.</p></body></html>";
        private static IEnumerable<Entity.Wiki> WikisField;
        public static IEnumerable<Entity.Wiki> Wikis { get => WikisField; set => WikisField = value; }

        public static void Init()
        {
            WikisField = new List<Entity.Wiki>
            {
               new Entity.Wiki{ Title="Title One", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Two", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Three", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Four", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Five", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Six", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Seven", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Eight", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Nine", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Ten", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Eleven", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Twelve", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Thirteen", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Fourteen", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Fifteen", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Sixteen", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Seventeen", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Eighteen", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Nineteen", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Twenty", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Twenty-One", IconPath="icon.png"},
               new Entity.Wiki{ Title="Title Twenty-Two", IconPath="icon.png"}
            };

            foreach (var item in WikisField)
                item.Content = string.Format(WIKICONTENTS, item.Title);
        }
    }
}
