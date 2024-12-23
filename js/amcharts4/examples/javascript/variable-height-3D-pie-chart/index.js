am4core.useTheme(am4themes_animated);

var chart = am4core.create("chartdiv", am4charts.PieChart3D);
chart.hiddenState.properties.opacity = 0; // this makes initial fade in effect

chart.data = [{
	"country": "Lithuania",
	"litres": 501.9
}, {
	"country": "Czech Republic",
	"litres": 301.9
}, {
	"country": "Ireland",
	"litres": 201.1
}, {
	"country": "Germany",
	"litres": 165.8
}, {
	"country": "Australia",
	"litres": 139.9
}, {
	"country": "Austria",
	"litres": 128.3
}];

chart.innerRadius = am4core.percent(40);
chart.depth = 90;

chart.legend = new am4charts.Legend();
chart.legend.position = "right";

var series = chart.series.push(new am4charts.PieSeries3D());
series.dataFields.value = "litres";
series.dataFields.depthValue = "litres";
series.dataFields.category = "country";