(function () {
    document.addEventListener('DOMContentLoaded', () => {

        // all destinations current year related elements
        const allDestCurrentYearShowAsTableBtn = document.getElementById('all-dest-current-year-show-btn');
        const allDestCurrentYearChartContainer = document.getElementById('all-dest-current-year-chart-container');
        const allDestCurrentYearTableContainer = document.getElementById('all-dest-current-year-table-container');

        // all destinations data over time related elements
        const allDestShowDataOverTimeBtn = document.getElementById('all-dest-show-data-over-time-btn');
        const allDestDataOverTimeChartContainer = document.getElementById('all-dest-data-over-time-chart-container');
        const allDestDataOverTimeTableContainer = document.getElementById('all-dest-data-over-time-table-container');
        const allDestDataOverTimeShowAsTableBtn = document.getElementById('all-dest-data-over-time-show-btn');

        const currentViewRadio = document.getElementById('current-view');
        const dataOvertimeViewRadio = document.getElementById('data-overtime-view');
        const allDestShowCurrentDataBtn = document.getElementById('all-dest-show-current-data-btn');

        setAriaAttribute(allDestCurrentYearShowAsTableBtn, 'false');
        if (allDestCurrentYearShowAsTableBtn) {
            allDestCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = allDestCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allDestCurrentYearChartContainer, allDestCurrentYearTableContainer, chartVisible, allDestCurrentYearShowAsTableBtn);
            });
        }

        if (allDestShowDataOverTimeBtn) {
            allDestShowDataOverTimeBtn.addEventListener('click', () => {
                if (dataOvertimeViewRadio) {
                    dataOvertimeViewRadio.checked = true;
                }

                var chartVisible = allDestCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allDestDataOverTimeChartContainer, allDestDataOverTimeTableContainer, !chartVisible, allDestDataOverTimeShowAsTableBtn);
                blurElementIfFocused(allDestShowDataOverTimeBtn);
                moveFocusToElement(allDestShowCurrentDataBtn);
            });
        }

        setAriaAttribute(allDestDataOverTimeShowAsTableBtn, 'false');
        if (allDestDataOverTimeShowAsTableBtn) {
            allDestDataOverTimeShowAsTableBtn.addEventListener('click', () => {
                const dataOverTimeChartVisible = allDestDataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(allDestDataOverTimeChartContainer, allDestDataOverTimeTableContainer, dataOverTimeChartVisible, allDestDataOverTimeShowAsTableBtn);
            });
        }

        if (allDestShowCurrentDataBtn) {
            allDestShowCurrentDataBtn.addEventListener('click', () => {
                if (currentViewRadio) {
                    currentViewRadio.checked = true;
                }

                var chartVisible = allDestDataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(allDestCurrentYearChartContainer, allDestCurrentYearTableContainer, !chartVisible, allDestCurrentYearShowAsTableBtn);
                blurElementIfFocused(allDestShowCurrentDataBtn);
                moveFocusToElement(allDestShowDataOverTimeBtn);
            });
        }

        addKeyboardFocusTransfer(allDestShowDataOverTimeBtn, allDestShowCurrentDataBtn);
        addKeyboardFocusTransfer(allDestShowCurrentDataBtn, allDestShowDataOverTimeBtn);

        const breakdownDestCurrentYearShowAsTableBtn = document.getElementById('breakdown-dest-current-year-show-btn');
        const breakdownDestCurrentYearChartContainer = document.getElementById('breakdown-dest-current-year-chart-container');
        const breakdownDestCurrentYearTableContainer = document.getElementById('breakdown-dest-current-year-table-container');

        setAriaAttribute(breakdownDestCurrentYearShowAsTableBtn, 'false');
        if (breakdownDestCurrentYearShowAsTableBtn) {
            breakdownDestCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = breakdownDestCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(breakdownDestCurrentYearChartContainer, breakdownDestCurrentYearTableContainer, chartVisible, breakdownDestCurrentYearShowAsTableBtn);
            });
        }
    });

    function setToggleText(toggle, text) {
        if (toggle) toggle.textContent = text;
    }

    function setAriaAttribute(toggle, text) {
        if (toggle) toggle.setAttribute('aria-expanded', text);
    }

    function blurElementIfFocused(element) {
        if (!element || document.activeElement !== element) {
            return;
        }

        element.blur();
    }

    function moveFocusToElement(element) {
        if (!element) {
            return;
        }

        [0, 50, 150, 300].forEach(delay => {
            setTimeout(() => {
                if (document.activeElement !== element) {
                    element.focus();
                }
            }, delay);
        });
    }

    function addKeyboardFocusTransfer(sourceElement, targetElement) {
        if (!sourceElement || !targetElement) {
            return;
        }

        sourceElement.addEventListener('keyup', (event) => {
            if (event.key !== 'Enter' && event.key !== ' ') {
                return;
            }

            moveFocusToElement(targetElement);
        });
    }

    function setTooggleState(chartContainer, tableContainer, isChartVisible, btnShow) {
        chartContainer.style.display = isChartVisible ? 'none' : 'block';
        tableContainer.style.display = isChartVisible ? 'block' : 'none';

        var isTableVisible = tableContainer.style.display === 'block';
        setToggleText(btnShow, isTableVisible ? 'Show as a chart' : 'Show as a table')
        setAriaAttribute(btnShow, isChartVisible ? 'true' : 'false');
    }

})();