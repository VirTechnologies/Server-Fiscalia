am4core.useTheme(am4themes_animated);

// create chart
var chart = am4core.create("chartdiv", am4charts.TreeMap);
chart.hiddenState.properties.opacity = 0;

chart.data = [{
	name: "First",
	children: [
		{
			name: "A1",
			value: 100
		},
		{
			name: "A2",
			value: 60
		},
		{
			name: "A3",
			value: 30
		}
	]
},
{
	name: "Second",
	children: [
		{
			name: "B1",
			value: 135
		},
		{
			name: "B2",
			value: 98
		},
		{
			name: "B3",
			value: 56
		}
	]
},
{
	name: "Third",
	children: [
		{
			name: "C1",
			value: 335
		},
		{
			name: "C2",
			value: 148
		},
		{
			name: "C3",
			value: 126
		},
		{
			name: "C4",
			value: 26
		}
	]
},
{
	name: "Fourth",
	children: [
		{
			name: "D1",
			value: 415
		},
		{
			name: "D2",
			value: 148
		},
		{
			name: "D3",
			value: 89
		},
		{
			name: "D4",
			value: 64
		},
		{
			name: "D5",
			value: 16
		}
	]
},
{
	name: "Fifth",
	children: [
		{
			name: "E1",
			value: 687
		},
		{
			name: "E2",
			value: 148
		}
	]
}];

chart.colors.step = 2;

// define data fields
chart.dataFields.value = "value";
chart.dataFields.name = "name";
chart.dataFields.children = "children";
chart.layoutAlgorithm = chart.binaryTree;

chart.zoomable = false;

// level 0 series template
var level0SeriesTemplate = chart.seriesTemplates.create("0");
var level0ColumnTemplate = level0SeriesTemplate.columns.template;

level0ColumnTemplate.column.cornerRadius(10, 10, 10, 10);
level0ColumnTemplate.fillOpacity = 0;
level0ColumnTemplate.strokeWidth = 4;
level0ColumnTemplate.strokeOpacity = 0;

// level 1 series template
var level1SeriesTemplate = chart.seriesTemplates.create("1");
level1SeriesTemplate.tooltip.dy = - 15;
level1SeriesTemplate.tooltip.pointerOrientation = "vertical";

var level1ColumnTemplate = level1SeriesTemplate.columns.template;

level1SeriesTemplate.tooltip.animationDuration = 0;
level1SeriesTemplate.strokeOpacity = 1;

level1ColumnTemplate.column.cornerRadius(10, 10, 10, 10)
level1ColumnTemplate.fillOpacity = 1;
level1ColumnTemplate.strokeWidth = 4;
level1ColumnTemplate.stroke = am4core.color("#ffffff");

var bullet1 = level1SeriesTemplate.bullets.push(new am4charts.LabelBullet());
bullet1.locationY = 0.5;
bullet1.locationX = 0.5;
bullet1.label.text = "{name}";
bullet1.label.fill = am4core.color("#ffffff");
bullet1.interactionsEnabled = false;
chart.maxLevels = 2;


setInterval(function () {
	for (var i = 0; i < chart.dataItems.length; i++) {
		var dataItem = chart.dataItems.getIndex(i);
		for (var c = 0; c < dataItem.children.length; c++) {
			var child = dataItem.children.getIndex(c);
			child.value = child.value + Math.round(child.value * Math.random() * 0.4 - 0.2);
		}
	}
}, 2000)