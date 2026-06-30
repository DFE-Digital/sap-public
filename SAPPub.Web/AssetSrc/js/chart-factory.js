(function () {
    const CHART_CONFIG = {
        defaults: {
            axisSuffix: '%',
            axisStepSize: 20,
            maxDevicePixelRatio: 2,
            maxCharsLength: 45,
            labelWrapChars: 15,
            mobileBreakpoint: '(max-width: 40.0625em)'            
        },
        bar: {
            labels: {
                yTickPadding: 10,
                baseContainerHeight: 260,
                defaultRowHeight: 20,
                minRowHeight: 40,
                rowHeight: 70,
                lineHeight: 18
            },
            dataset: {
                borderWidth: 1,
                barThickness: 'flex',
                maxBarThickness: 70,
                minBarLength: 3,
                categoryPercentage: 0.8,
                barPercentage: 0.9
            },
            datalabels: {
                anchor: 'end',
                smallValueAlign: 'end',
                defaultAlign: 'start',
                mobileInsideThresholdRatio: 0.45,
                offset: 5,
                fontWeight: 'bold',
                showDataLabels: true
            },            
            noData: {
                text: 'Not available'
            }
        },
        defaultColors: [
            '#A285D1',
            '#12436D',
            '#28A197'
        ]
    };

    const charts = {};

    function gdsVars(canvas) {
        const s = getComputedStyle(canvas);

        return {
            fontFamily: s.getPropertyValue('--gds-font-family').trim(),
            fontSize: parseInt(s.getPropertyValue('--gds-font-size')),
            text: s.getPropertyValue('--gds-text'),
            grey: s.getPropertyValue('--gds-grey'),
            labelBg: s.getPropertyValue('--gds-label-bg'),
            labelBorder: s.getPropertyValue('--gds-label-border'),
            labelPadding: parseInt(s.getPropertyValue('--gds-label-padding'))
        };
    }

    function isMobileViewport() {
        return window.matchMedia(CHART_CONFIG.defaults.mobileBreakpoint).matches;
    }

    function resizeBarChartContainer(canvas, chartData) {
        const container = canvas.parentElement;

        var dataPointsLength = 0;
        if (Array.isArray(chartData.datasets)) {
            dataPointsLength = chartData.datasets.reduce((sum, dataset) => sum + (dataset.data?.length ?? 0),0);
        }
        else {
            dataPointsLength = chartData.labels.length;
        }

        const labels = Array.isArray(chartData.labels) ? chartData.labels : [];

        if (!container || labels.length === 0) {
            return;
        }

        const maxWrappedLines = Math.max(...labels.map(label =>
            wrapLabel(label.toString(), CHART_CONFIG.defaults.labelWrapChars).length
        ));

        const calculatedRowHeight = dataPointsLength > 9 ? CHART_CONFIG.bar.labels.defaultRowHeight : CHART_CONFIG.bar.labels.rowHeight;

        var rowHeight = calculatedRowHeight + Math.max(0, maxWrappedLines - 2) * CHART_CONFIG.bar.labels.lineHeight;

        if (rowHeight <= CHART_CONFIG.bar.labels.minRowHeight) {
            rowHeight = CHART_CONFIG.bar.labels.minRowHeight;
        }

        const height = Math.max(CHART_CONFIG.bar.labels.baseContainerHeight, dataPointsLength * rowHeight);
        container.style.height = `${height}px`;
    }

    function getBarLabelAlignment(ctx, axisSuffix) {
        if (isMobileViewport()) {
            return isLargeEnoughForInsideLabel(ctx.dataset.data[ctx.dataIndex], ctx)
                ? CHART_CONFIG.bar.datalabels.defaultAlign
                : CHART_CONFIG.bar.datalabels.smallValueAlign;
        }

        return canBarFitLabel(ctx, axisSuffix)
            ? CHART_CONFIG.bar.datalabels.defaultAlign
            : CHART_CONFIG.bar.datalabels.smallValueAlign;
    }

    function canBarFitLabel(ctx, axisSuffix) {
        const value = ctx.dataset.data[ctx.dataIndex];
        if (value === null || value === undefined || Number.isNaN(value)) {
            return false;
        }

        const xScale = ctx.chart?.scales?.x;
        const canvasContext = ctx.chart?.ctx;
        if (!xScale || !canvasContext) {
            return true;
        }

        const barLength = Math.abs(xScale.getPixelForValue(value) - xScale.getPixelForValue(0));
        const font = Chart.helpers.toFont(ctx.chart.options?.plugins?.datalabels?.font);

        canvasContext.save();
        canvasContext.font = font.string;
        const labelWidth = canvasContext.measureText(getBarLabelText(value, axisSuffix)).width;
        canvasContext.restore();

        return barLength >= labelWidth + (CHART_CONFIG.bar.datalabels.offset * 2);
    }

    function getBarLabelText(value, axisSuffix) {
        if (value === null || value === undefined || Number.isNaN(value)) {
            return '';
        }
        return `${value}${axisSuffix}`;
    }

    function isLargeEnoughForInsideLabel(value, ctx) {
        if (value === null || value === undefined || Number.isNaN(value)) {
            return false;
        }

        const xScale = ctx.chart?.scales?.x;
        const axisMin = xScale?.min ?? 0;
        const axisMax = xScale?.max ?? 0;
        const axisRange = axisMax - axisMin;

        if (axisRange <= 0) {
            return true;
        }

        return ((value - axisMin) / axisRange) >= CHART_CONFIG.bar.datalabels.mobileInsideThresholdRatio;
    }

    function getBarLabelColor(ctx, gdsStyles) {
        const bg = Array.isArray(ctx.dataset.backgroundColor) ? ctx.dataset.backgroundColor[ctx.dataIndex] : ctx.dataset.backgroundColor;
        const align = getBarLabelAlignment(ctx, CHART_CONFIG.defaults.axisSuffix);
        return align === CHART_CONFIG.bar.datalabels.defaultAlign && bg === '#12436D' ? '#ffffff' : gdsStyles.text;
    }

    function wrapLabel(label, labelWrapChars) {

        if (label.length > CHART_CONFIG.defaults.maxCharsLength) {
            label = `${label.substring(0, CHART_CONFIG.defaults.maxCharsLength)}...`;
        }

        const words = label.split(' ');
        const lines = [];
        let line = '';

        words.forEach(word => {
            if ((line + word).length > labelWrapChars) {
                lines.push(line.trim());
                line = word + ' ';
            } else {
                line += word + ' ';
            }
        });

        lines.push(line.trim());
        return lines;
    }

    function buildChartOptions(type, gdsStyles, notAvailableText) {
        const common = {
            responsive: true,
            maintainAspectRatio: false,
            devicePixelRatio: Math.min(window.devicePixelRatio || 1, CHART_CONFIG.defaults.maxDevicePixelRatio)            
        };

        const fonts = {
            family: gdsStyles.fontFamily,
            size: gdsStyles.fontSize
        }

        const legendOptions = { display: false };

        if (type === 'bar') {
            return {
                ...common,                
                indexAxis: 'y',
                scales: {
                    x: {
                        beginAtZero: true,
                        //max: 100,
                        grid: {
                            display: true,
                            drawBorder: false,
                            color: (context) => {
                                return context.tick.value === 0 ? '#000' : '#ccc';
                            },
                            lineWidth: (context) => {
                                return context.tick.value === 0 ? 2 : 1;
                            }
                        },
                        border: { display: false },
                        ticks: {
                            color: gdsStyles.text,
                            font: fonts,
                            stepSize: CHART_CONFIG.defaults.axisStepSize,
                            callback: (value) => `${value}${CHART_CONFIG.defaults.axisSuffix}`
                        }
                    },
                    y: {
                        grid: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            color: gdsStyles.text,
                            font: fonts,
                            callback: function (value) {
                                const label = this.getLabelForValue(value);
                                return wrapLabel(label.toString(), CHART_CONFIG.defaults.labelWrapChars);
                            },
                            padding: CHART_CONFIG.bar.labels.yTickPadding
                        }
                    }
                },
                plugins: {
                    tooltip: { enabled: false },
                    legend: legendOptions,
                    title: {
                        display: false,
                        font: fonts
                    },
                    datalabels: {
                        anchor: CHART_CONFIG.bar.datalabels.anchor,
                        align: function (ctx) {
                            return getBarLabelAlignment(ctx, CHART_CONFIG.defaults.axisSuffix);
                        },
                        offset: CHART_CONFIG.bar.datalabels.offset,
                        color: (ctx) => {
                            return getBarLabelColor(ctx, gdsStyles);                            
                        },
                        font: {
                            ...fonts,
                            weight: CHART_CONFIG.bar.datalabels.fontWeight
                        },
                        display: CHART_CONFIG.bar.datalabels.showDataLabels,
                        formatter: function (value) {
                            return value === null ? notAvailableText : `${value}${CHART_CONFIG.defaults.axisSuffix}`;
                        },
                        clamp: true,
                        clip: false
                    }
                }
            };
        }
        return common;
    }

    function buildDatasets(type, chartData, colors) {
        if (type === 'bar') {
            const dataOptions = {                
                ...CHART_CONFIG.bar.dataset
            }

            if (Array.isArray(chartData.datasets)) {
                return chartData.datasets.map((ds, i) => ({
                    label: ds.label,
                    data: ds.data,
                    backgroundColor: colors[i] || CHART_CONFIG.defaultColors[i],
                    ...dataOptions
                }));
            }

            return [{
                data: chartData.data,
                backgroundColor: colors.length ? colors : CHART_CONFIG.defaultColors,
                ...dataOptions               
            }];
        }
    }

    function initCharts() {
        document.querySelectorAll('.js-chart').forEach(canvas => {

            if (charts[canvas.id]) {
                charts[canvas.id].destroy();
            }

            const gdsStyles = gdsVars(canvas);

            Chart.defaults.font.fontFamily = gdsStyles.fontFamily;
            Chart.defaults.font.size = gdsStyles.fontSize;
            Chart.defaults.color = gdsStyles.text;

            const chartData = JSON.parse(canvas.dataset.chart);
            const type = canvas.dataset.type;
            const showLegend = canvas.dataset.showLegend === "true";
            const notAvailableText = canvas.dataset.notAvailableText || CHART_CONFIG.bar.noData.text;

            const colors = canvas.dataset.colors
                ? JSON.parse(canvas.dataset.colors)
                : [];

            const config = {
                type,
                data: {
                    labels: chartData.labels,
                    datasets: buildDatasets(type, chartData, colors)
                },
                options: buildChartOptions(type, gdsStyles, notAvailableText),
                plugins: [ChartDataLabels]
            };

            resizeBarChartContainer(canvas, chartData);

            var chart = new Chart(canvas, config);

            charts[canvas.id] = chart;           

            if (showLegend) {
                const legendContainer = document.querySelector(`.chart-legend[data-chart-id="${canvas.id}"]`);
                if (legendContainer) {
                    buildVerticalLegend(chart, legendContainer)
                }
            }
        });
    }  

    function buildVerticalLegend(chart, container) {

        const datasets = Array.isArray(chart.data.datasets) ? chart.data.datasets : [chart.data.datasets];

        const ul = document.createElement('ul');

        datasets.forEach(ds => {
            const li = document.createElement('li');
            const box = document.createElement("span");
            box.classList.add("legend-box");
            box.style.backgroundColor = ds.backgroundColor || ds.borderColor || "#000";

            const label = document.createElement("span");
            label.textContent = ds.label;

            li.appendChild(box);
            li.appendChild(label);
            ul.appendChild(li);
        });

        container.appendChild(ul);
    }

    function adjustChartResize() {
        let resizeTimeout;
        window.addEventListener('resize', () => {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(() => {
                Object.values(charts).forEach(chart => {
                    const fontSizePx = gdsVars(chart.canvas).fontSize;

                    if (chart.options.scales.x.ticks.font)
                        chart.options.scales.x.ticks.font.size = fontSizePx;

                    if (chart.options.scales.y.ticks.font)
                        chart.options.scales.y.ticks.font.size = fontSizePx;

                    if (chart.options.plugins.title.font)
                        chart.options.plugins.title.font.size = fontSizePx;

                    if (chart.options.plugins.datalabels.font)
                        chart.options.plugins.datalabels.font.size = fontSizePx;
                    chart.update();
                });
            }, 100);
        });
    }
    document.addEventListener('DOMContentLoaded', initCharts);
    
    adjustChartResize();
})();