$(function () {
    var chartData;
    var apexChart;
    function fetchData() {
        $.ajax({
            url: 'Admin/Dashboard/GetChartData',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                chartData = data;
                renderChartLine(chartData);
                renderChartArea(chartData);
                renderChart(getCurrentMonthValue());
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    fetchData();
    function renderChart(selectedMonth) {
        var filteredData = chartData.filter(function (item) {
            return item.month.startsWith(selectedMonth);
        });
        var mergedData = [];
        filteredData.forEach(function (item) {
            var existingData = mergedData.find(function (dataItem) {
                var date = new Date(dataItem.month);
                return date.getDate() === new Date(item.month).getDate();
            });

            if (existingData) {
                existingData.earnings += item.earnings;
                existingData.expenses += item.expenses;
            } else {
                mergedData.push({
                    month: item.month,
                    earnings: item.earnings,
                    expenses: item.expenses
                });
            }
        });
        var earningsData = mergedData.map(function (item) {
            return item.earnings;
        });
        var expensesData = mergedData.map(function (item) {
            return item.expenses;
        });
        var dateLabels = mergedData.map(function (item) {
            var date = new Date(item.month);
            return date.getDate() + "/" + (date.getMonth() + 1);
        });
        dateLabels.sort(function (a, b) {
            return a.split("/")[0] - b.split("/")[0];
        });
        if (apexChart) {
            apexChart.updateOptions({
                series: [
                    { name: "Earnings this month:", data: earningsData },
                    { name: "Expense this month:", data: expensesData },
                ],                   
                xaxis: {
                    type: "category",
                    categories: dateLabels,
                    labels: {
                        style: { cssClass: "grey--text lighten-2--text fill-color" },
                    },
                },               
            });
        }
        else {
            var chart = {
                series: [
                    { name: "Earnings this month:", data: earningsData },
                    { name: "Expense this month:", data: expensesData },
                ],
                chart: {
                    type: "bar",
                    height: 345,
                    offsetX: -15,
                    toolbar: { show: true },
                    foreColor: "#adb0bb",
                    fontFamily: 'inherit',
                    sparkline: { enabled: false },
                },
                colors: ["#5D87FF", "#49BEFF"],
                plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: "35%",
                        borderRadius: [6],
                        borderRadiusApplication: 'end',
                        borderRadiusWhenStacked: 'all'
                    },
                },
                markers: { size: 0 },
                dataLabels: {
                    enabled: false,
                },
                legend: {
                    show: false,
                },
                grid: {
                    borderColor: "rgba(0,0,0,0.1)",
                    strokeDashArray: 3,
                    xaxis: {
                        lines: {
                            show: false,
                        },
                    },
                },
                xaxis: {
                    type: "category",
                    categories: dateLabels,
                    labels: {
                        style: { cssClass: "grey--text lighten-2--text fill-color" },
                    },
                },
                yaxis: {
                    show: true,
                    min: 0,
                    max: 400,
                    tickAmount: 4,
                    labels: {
                        style: {
                            cssClass: "grey--text lighten-2--text fill-color",
                        },
                    },
                },
                stroke: {
                    show: true,
                    width: 3,
                    lineCap: "butt",
                    colors: ["transparent"],
                },
                tooltip: { theme: "light" },
                responsive: [
                    {
                        breakpoint: 600,
                        options: {
                            plotOptions: {
                                bar: {
                                    borderRadius: 3,
                                }
                            },
                        }
                    }
                ]
            };
            apexChart = new ApexCharts(document.querySelector("#chartt"), chart);
            apexChart.render();
        };
        
    }
    $('#monthSelection').on('change', function () {
        var selectedMonth = $(this).val();
        renderChart(selectedMonth);
    });
    function getCurrentMonthValue() {
        var currentDate = new Date();
        var currentMonth = currentDate.getMonth() + 1;
        var currentYear = currentDate.getFullYear();
        var formattedValue = `${currentYear}-${currentMonth.toString().padStart(2, '0')}`;
        return formattedValue;
    }
    
    // ================ Breakup =====================//
    function renderChartLine(chartData) {
        var yearlyData = {};
        chartData.forEach(function (item) {
            var year = new Date(item.month).getFullYear();
            if (yearlyData[year]) {
                yearlyData[year].earnings += item.earnings;
                yearlyData[year].expenses += item.expenses;
            } else {
                yearlyData[year] = {
                    earnings: item.earnings,
                    expenses: item.expenses
                };
            }
        });
        var series = Object.values(yearlyData).map(function (data) {
            return data.earnings;
        });
        var labels = Object.keys(yearlyData);

        var breakup = {
            color: "#adb5bd",
            series: series,
            labels: labels,
            chart: {
                width: 180,
                type: "donut",
                fontFamily: "Plus Jakarta Sans', sans-serif",
                foreColor: "#adb0bb",
            },
            plotOptions: {
                pie: {
                    startAngle: 0,
                    endAngle: 360,
                    donut: {
                        size: '75%',
                    },
                },
            },
            stroke: {
                show: false,
            },

            dataLabels: {
                enabled: false,
            },

            legend: {
                show: false,
            },
            colors: ["#008080", "#000080","#FF0000", "#FFFF00", "#008000", "#0000FF","#800080","#FFC0CB","#FFD700","#800000","#00FF00",],     
            responsive: [
                {
                    breakpoint: 991,
                    options: {
                        chart: {
                            width: 150,
                        },
                    },
                },
            ],
            tooltip: {
                theme: "dark",
                fillSeriesColor: false,
            },
        };
        var chart = new ApexCharts(document.querySelector("#breakupp"), breakup);
        chart.render();
    }
    // Earning
    // =====================================
    function renderChartArea(chartData)
    {
        var currentDate = new Date();
        var currentMonth = currentDate.getMonth() + 1;
        var currentYear = currentDate.getFullYear();
        var filteredData = chartData.filter(function (item) {
            var date = new Date(item.month);
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            return month === currentMonth && year === currentYear;
        });
        var earningsData = filteredData.map(function (item) {
            return item.earnings;
        });

        var earning = {
            chart: {
                id: "sparkline3",
                type: "area",
                height: 60,
                sparkline: {
                    enabled: true,
                },
                group: "sparklines",
                fontFamily: "Plus Jakarta Sans', sans-serif",
                foreColor: "#adb0bb",
            },
            series: [
                {
                    name: "Earnings",
                    color: "#49BEFF",
                    data: earningsData ,
                },
            ],
            stroke: {
                curve: "smooth",
                width: 2,
            },
            fill: {
                colors: ["#f3feff"],
                type: "solid",
                opacity: 0.05,
            },

            markers: {
                size: 0,
            },
            tooltip: {
                theme: "dark",
                fixed: {
                    enabled: true,
                    position: "right",
                },
                x: {
                    show: false,
                },
            },
        };
        new ApexCharts(document.querySelector("#earning"), earning).render();
    }
   
});

 
