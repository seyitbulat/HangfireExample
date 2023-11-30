using HtmlAgilityPack;
using System.Globalization;
using System.Xml;

namespace TestService
{
    public class CurrencyService
    {
        public HttpClient _client { get; set; }
        public CurrencyService(HttpClient client)
        {
            _client = client;
        }
        public async Task<List<Currency>> GetCurrencies()
        {
            var response = await _client.GetAsync("https://www.tcmb.gov.tr/kurlar/today.xml");

            var xml = await response.Content.ReadAsStringAsync();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var currencyNodes = doc.SelectNodes("/Tarih_Date/Currency");

            var dolarNode = currencyNodes.Cast<XmlNode>().FirstOrDefault(node =>
                node.SelectSingleNode("@CurrencyCode")?.InnerText == "USD"
            );

            var euroNode = currencyNodes.Cast<XmlNode>().FirstOrDefault(node =>
                node.SelectSingleNode("@CurrencyCode")?.InnerText == "EUR"
            );

            var dollarDto = new Currency
            {
                Type = dolarNode?.SelectSingleNode("CurrencyName")?.InnerText,
                Buy = string.IsNullOrEmpty(dolarNode?.SelectSingleNode("ForexBuying")?.InnerText) ? 0.0m :
                       decimal.Parse(dolarNode?.SelectSingleNode("ForexBuying")?.InnerText, CultureInfo.InvariantCulture),
                Sell = string.IsNullOrEmpty(dolarNode?.SelectSingleNode("ForexSelling")?.InnerText) ? 0.0m :
                       decimal.Parse(dolarNode?.SelectSingleNode("ForexSelling")?.InnerText, CultureInfo.InvariantCulture),
            };

            var euroDto = new Currency
            {
                Type = euroNode?.SelectSingleNode("CurrencyName")?.InnerText,
                Buy = string.IsNullOrEmpty(euroNode?.SelectSingleNode("ForexBuying")?.InnerText) ? 0.0m :
                       decimal.Parse(euroNode?.SelectSingleNode("ForexBuying")?.InnerText, CultureInfo.InvariantCulture),
                Sell = string.IsNullOrEmpty(dolarNode?.SelectSingleNode("ForexSelling")?.InnerText) ? 0.0m :
                       decimal.Parse(dolarNode?.SelectSingleNode("ForexSelling")?.InnerText, CultureInfo.InvariantCulture),
            };

            var returnList = new List<Currency>() { dollarDto, euroDto};
            return returnList;
        }

        public async Task<List<Currency>> GetCurrencies2()
        {
            var result = new List<Currency>();

            var response = await _client.GetAsync("https://canlidoviz.com/");
            var html = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Adjust the XPath expression based on the structure of the HTML on the website
            var nodes = doc.DocumentNode.SelectNodes("//td[@class='canli text-mobile-value']");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var currencyName = node.SelectSingleNode(".//span[@class='currency']")?.InnerText;
                    var exchangeRate = node.SelectSingleNode(".//span[@class='value']")?.InnerText;

                    if (!string.IsNullOrEmpty(currencyName) && !string.IsNullOrEmpty(exchangeRate))
                    {
                        var currency = new Currency
                        {
                            Type = currencyName.Trim(),
                            Buy =  Decimal.Parse(exchangeRate.Trim())
                        };

                        result.Add(currency);
                    }
                }
            }

            return result;
        }
    }
}
