const colors = ["#1f77b4", "#ff7f0e", "#2ca02c", "#d62728", "#9467bd"];

function renderChart(data) {

  data.sort((a, b) => a - b);

  d3.select("#chart").selectAll("*").remove();

  const width = 400;
  const barHeight = 30;

  const xScale = d3.scaleLinear()
    .domain([0, d3.max(data)])
    .range([0, width]);

  const svg = d3.select("#chart")
    .append("svg")
    .attr("width", width + 50)
    .attr("height", data.length * (barHeight + 5));

  svg.selectAll("rect")
    .data(data)
    .enter()
    .append("rect")
    .attr("x", 0)
    .attr("y", (d, i) => i * (barHeight + 5))
    .attr("width", d => xScale(d))
    .attr("height", barHeight)
    .attr("fill", (d, i) => colors[i % colors.length]);

  svg.selectAll("text")
    .data(data)
    .enter()
    .append("text")
    .attr("x", d => xScale(d) - 5)
    .attr("y", (d, i) => i * (barHeight + 5) + barHeight / 1.6)
    .attr("fill", "white")
    .attr("text-anchor", "end")
    .text(d => d);
}

document.getElementById("updateBtn").addEventListener("click", () => {
  const input = document.getElementById("sourceData").value;
  const numbers = input.split(",")
    .map(n => parseInt(n.trim()))
    .filter(n => !isNaN(n));

  if (numbers.length > 0) {
    renderChart(numbers);
  }
});

renderChart([4, 8, 15, 16]);
