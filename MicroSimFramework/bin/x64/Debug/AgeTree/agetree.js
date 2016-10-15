
var data = [
    { group: '0-9', male: 10, female: 12 },
    { group: '10-19', male: 14, female: 15 },
    { group: '20-29', male: 15, female: 18 },
    { group: '30-39', male: 18, female: 18 },
    { group: '40-49', male: 21, female: 22 },
    { group: '50-59', male: 19, female: 24 },
    { group: '60-69', male: 15, female: 14 },
    { group: '70-79', male: 8, female: 10 },
    { group: '80-89', male: 4, female: 5 },
    { group: '90-99', male: 2, female: 3 },
    { group: '100-109', male: 1, female: 1 },
];


//var data = [];
var year = 0;

// GET THE TOTAL POPULATION SIZE AND CREATE A FUNCTION FOR RETURNING THE PERCENTAGE
var totalPopulation = d3.sum(data, function (d) { return d.male + d.female; }),
    percentage = function (d) { return d / totalPopulation; };

// find the maximum data value on either side
//  since this will be shared by both of the x-axes
var maxValue = Math.max(
    d3.max(data, function (d) { return percentage(d.male); }),
    d3.max(data, function (d) { return percentage(d.female); })
);

// margin.middle is distance from center line to each y-axis
var margin = {
    top: 40,
    right: 20,
    bottom: 30,
    left: 20,
    middle: 28
};

// CREATE SVG
var svg = d3.select('body')
    .append('svg')    
    // ADD A GROUP FOR THE SPACE WITHIN THE MARGINS
    .append('g')
    .attr('transform', translation(margin.left, margin.top));
var yearText = svg.append("text");
var maleText = svg.append("text").text("Male");
var femaleText = svg.append("text").text("Female");
var xScale = d3.scaleLinear();
var xScaleLeft = d3.scaleLinear();
var xScaleRight = d3.scaleLinear();
var yScale = d3.scaleBand();
var yAxisLeft = d3.axisRight();
var yAxisRight = d3.axisLeft();
var xAxisRight = d3.axisBottom();
var xAxisLeft = d3.axisBottom();
var leftBarGroup = svg.append('g');
var rightBarGroup = svg.append('g');
var a1 = svg.append('g');
var a2 = svg.append('g');
var a3 = svg.append('g');
var a4 = svg.append('g');
var leftBars = leftBarGroup.selectAll('.bar.left');
var rightBars = rightBarGroup.selectAll('.bar.right');
var leftRects = leftBars.data(data).enter().append('rect');
var rightRects = rightBars.data(data).enter().append('rect');

createAgeTree();

function setData(inputYear, inputData, xMax, yMax) {
    year = inputYear;
    data = inputData;
    maxValue = xMax;
    leftRects.remove();
    rightRects.remove();
    leftRects = leftBars.data(data).enter().append('rect');
    rightRects = rightBars.data(data).enter().append('rect');
    createAgeTree();
}

// Create the age tree
function createAgeTree() {
    svg.width = window.innerWidth;
    svg.height = window.innerHeight;
    var w = svg.width - margin.left - margin.right,
        h = svg.height - margin.top - margin.bottom;
    // svg.width = margin.left + w + margin.right;
    //svg.height = margin.top + h + margin.bottom;   

    // the width of each side of the chart
    var regionWidth = w / 2 - margin.middle;

    yearText.text(year)
            .attr("x", w / 2)
            .attr("y", -10)
            .attr("font-size", "30px")
            .attr("font-weight", "bold")
            .attr("text-anchor", "middle");
    maleText.attr("font-size", "20px")
            .attr("font-weight", "bold");
    femaleText.attr("x", w)
              .attr("font-size", "20px")
              .attr("font-weight", "bold")
              .attr("text-anchor", "end");

    // these are the x-coordinates of the y-axes
    var pointA = regionWidth,
        pointB = w - regionWidth;      

    // SET UP SCALES

    // the xScale goes from 0 to the width of a region
    //  it will be reversed for the left x-axis
    xScale
        .domain([0, maxValue])
        .range([0, regionWidth])
        .nice();
    
    xScaleLeft
        .domain([0, maxValue])
        .range([regionWidth, 0]);
    
    xScaleRight
        .domain([0, maxValue])
        .range([0, regionWidth]);
    
    yScale
        .domain(data.map(function (d) { return d.group; }))
        .range([h, 0], 0.1);


    // SET UP AXES
    yAxisLeft
        .scale(yScale)
        .tickSize(4, 0)
        .tickPadding(margin.middle - 4);
    
    yAxisRight
        .scale(yScale)
        .tickSize(4, 0)        
        .tickFormat('');
    
    xAxisRight
        .scale(xScale)
        .tickFormat(d3.format(''));
    
    xAxisLeft
        // REVERSE THE X-AXIS SCALE ON THE LEFT SIDE BY REVERSING THE RANGE
        .scale(xScale.copy().range([pointA, 0]))
        .tickFormat(d3.format(''));

    // MAKE GROUPS FOR EACH SIDE OF CHART
    // scale(-1,1) is used to reverse the left side so the bars grow left instead of right
    leftBarGroup
        .attr('transform', translation(pointA, 0) + 'scale(-1,1)');    
    rightBarGroup
        .attr('transform', translation(pointB, 0));

    // DRAW AXES
    a1
        .attr('class', 'axis y left')
        .attr('transform', translation(pointA, 0))
        .call(yAxisLeft)
        .selectAll('text')
        .style('text-anchor', 'middle');

    a2
        .attr('class', 'axis y right')
        .attr('transform', translation(pointB, 0))
        .call(yAxisRight);

    a3
        .attr('class', 'axis x left')
        .attr('transform', translation(0, h))
        .call(xAxisLeft);

    a4
        .attr('class', 'axis x right')
        .attr('transform', translation(pointB, h))
        .call(xAxisRight);

    // DRAW BARS
    leftRects
        .attr('class', 'bar left')
        .attr('x', 0)
        .attr('y', function (d) { return yScale(d.group); })
        .attr('width', function (d) { return xScale(d.male); })
        .attr('height', yScale.bandwidth());

    rightRects
        .attr('class', 'bar right')
        .attr('x', 0)
        .attr('y', function (d) { return yScale(d.group); })
        .attr('width', function (d) { return xScale(d.female); })
        .attr('height', yScale.bandwidth());

}

// so sick of string concatenation for translations
function translation(x, y) {
    return 'translate(' + x + ',' + y + ')';
}

window.onresize = function(event) {
    createAgeTree();
};