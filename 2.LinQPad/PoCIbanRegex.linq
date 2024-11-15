<Query Kind="Statements" />

var reg = new Regex("[A-Z]{2,2}[0-9]{2,2}[a-zA-Z0-9]{1,30}");
reg.IsMatch("AT611904300234573201")