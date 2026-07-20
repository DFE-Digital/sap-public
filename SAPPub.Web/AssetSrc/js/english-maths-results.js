(function () {
    document.addEventListener('DOMContentLoaded', () => {

        // all gcse current year related elements
        const allGcseCurrentYearShowAsTableBtn = document.getElementById('all-gcse-current-year-show-btn');
        const allGcseCurrentYearChartContainer = document.getElementById('all-gcse-current-year-chart-container');
        const allGcseCurrentYearTableContainer = document.getElementById('all-gcse-current-year-table-container');

        // all gcse data over time related elements
        const allGcseShowDataOverTimeBtn = document.getElementById('all-gcse-show-data-over-time-btn');
        const allGcseDataOverTimeChartContainer = document.getElementById('all-gcse-data-over-time-chart-container');
        const allGcseDataOverTimeTableContainer = document.getElementById('all-gcse-data-over-time-table-container');
        const allGcseDataOverTimeShowAsTableBtn = document.getElementById('all-gcse-data-over-time-show-btn');

        const currentViewRadio = document.getElementById('current-view');
        const dataOvertimeViewRadio = document.getElementById('data-overtime-view');
        const allGcseShowCurrentDataBtn = document.getElementById('all-gcse-show-current-data-btn');

        setAriaAttribute(allGcseCurrentYearShowAsTableBtn, 'false');

        if (allGcseCurrentYearShowAsTableBtn) {
            allGcseCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = allGcseCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allGcseCurrentYearChartContainer, allGcseCurrentYearTableContainer, chartVisible, allGcseCurrentYearShowAsTableBtn);
            });
        }

        if (allGcseShowDataOverTimeBtn) {
            allGcseShowDataOverTimeBtn.addEventListener('click', () => {
                if (dataOvertimeViewRadio) {
                    dataOvertimeViewRadio.checked = true;
                }

                var chartVisible = allGcseCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allGcseDataOverTimeChartContainer, allGcseDataOverTimeTableContainer, !chartVisible, allGcseDataOverTimeShowAsTableBtn);
                blurElementIfFocused(allGcseShowDataOverTimeBtn);
                moveFocusToElement(allGcseShowCurrentDataBtn);
            });
        }

        setAriaAttribute(allGcseDataOverTimeShowAsTableBtn, 'false');

        if (allGcseDataOverTimeShowAsTableBtn) {
            allGcseDataOverTimeShowAsTableBtn.addEventListener('click', () => {
                const dataOverTimeChartVisible = allGcseDataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(allGcseDataOverTimeChartContainer, allGcseDataOverTimeTableContainer, dataOverTimeChartVisible, allGcseDataOverTimeShowAsTableBtn);
            });
        }

        if (allGcseShowCurrentDataBtn) {
            allGcseShowCurrentDataBtn.addEventListener('click', () => {
                if (currentViewRadio) {
                    currentViewRadio.checked = true;
                }

                var chartVisible = allGcseDataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(allGcseCurrentYearChartContainer, allGcseCurrentYearTableContainer, !chartVisible, allGcseCurrentYearShowAsTableBtn);
                blurElementIfFocused(allGcseShowCurrentDataBtn);
                moveFocusToElement(allGcseShowDataOverTimeBtn);
            });
        }

        addKeyboardFocusTransfer(allGcseShowDataOverTimeBtn, allGcseShowCurrentDataBtn);
        addKeyboardFocusTransfer(allGcseShowCurrentDataBtn, allGcseShowDataOverTimeBtn);

        addEnterKeyHandler(allGcseShowDataOverTimeBtn);
        addEnterKeyHandler(allGcseShowCurrentDataBtn);

        const breakdownGcseCurrentYearShowAsTableBtn = document.getElementById('breakdown-gcse-current-year-show-btn');
        const breakdownGcseCurrentYearChartContainer = document.getElementById('breakdown-gcse-current-year-chart-container');
        const breakdownGcseCurrentYearTableContainer = document.getElementById('breakdown-gcse-current-year-table-container');

        setAriaAttribute(breakdownGcseCurrentYearShowAsTableBtn, 'false');

        if (breakdownGcseCurrentYearShowAsTableBtn) {
            breakdownGcseCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = breakdownGcseCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(breakdownGcseCurrentYearChartContainer, breakdownGcseCurrentYearTableContainer, chartVisible, breakdownGcseCurrentYearShowAsTableBtn);
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

    function addEnterKeyHandler(element) {
        if (!element) {
            return;
        }

        element.addEventListener('keydown', (event) => {
            if (event.key === 'Enter' || event.key === ' ') {
                event.preventDefault();
                element.click();
            }
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