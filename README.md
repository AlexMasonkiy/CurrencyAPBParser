# CurrencyAPBParser
get all currency from agroprombank by date

need install RestSharp from http://restsharp.org/

<b>example using:</b>

  var parser = new CurrencyApbParser();
  
  var currencies = parser.GetAllToday();
