(function () {
    document.addEventListener('DOMContentLoaded', () => {

        // all gcse current year related elements
        const readCurrentYearShowAsTableBtn = document.getElementById('read-current-year-show-btn');
        const readCurrentYearChartContainer = document.getElementById('read-current-year-chart-container');
        const readCurrentYearTableContainer = document.getElementById('read-current-year-table-container');

        // all gcse data over time related elements
        const readShowDataOverTimeBtn = document.getElementById('read-show-data-over-time-btn');
        const readDataOverTimeChartContainer = document.getElementById('read-data-over-time-chart-container');
        const readDataOverTimeTableContainer = document.getElementById('read-data-over-time-table-container');
        const readDataOverTimeShowAsTableBtn = document.getElementById('read-data-over-time-show-btn');

        const currentViewRadio = document.getElementById('current-view');
        const dataOvertimeViewRadio = document.getElementById('data-overtime-view');
        const readShowCurrentDataBtn = document.getElementById('read-show-current-data-btn');

        setAriaAttribute(readCurrentYearShowAsTableBtn, 'false');

        if (readCurrentYearShowAsTableBtn) {
            readCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = readCurrentYearChartContainer.style.display !== 'none';
                setToggleState(readCurrentYearChartContainer, readCurrentYearTableContainer, chartVisible, readCurrentYearShowAsTableBtn);
            });
        }

        if (readShowDataOverTimeBtn) {
            readShowDataOverTimeBtn.addEventListener('click', () => {
                if (dataOvertimeViewRadio) {
                    dataOvertimeViewRadio.checked = true;
                }

                var chartVisible = readCurrentYearChartContainer.style.display !== 'none';
                setToggleState(readDataOverTimeChartContainer, readDataOverTimeTableContainer, !chartVisible, readDataOverTimeShowAsTableBtn);
                blurElementIfFocused(readShowDataOverTimeBtn);
                moveFocusToElement(readShowCurrentDataBtn);
            });
        }

        setAriaAttribute(readDataOverTimeShowAsTableBtn, 'false');

        if (readDataOverTimeShowAsTableBtn) {
            readDataOverTimeShowAsTableBtn.addEventListener('click', () => {
                const dataOverTimeChartVisible = readDataOverTimeChartContainer.style.display !== 'none';
                setToggleState(readDataOverTimeChartContainer, readDataOverTimeTableContainer, dataOverTimeChartVisible, readDataOverTimeShowAsTableBtn);
            });
        }

        if (readShowCurrentDataBtn) {
            readShowCurrentDataBtn.addEventListener('click', () => {
                if (currentViewRadio) {
                    currentViewRadio.checked = true;
                }

                var chartVisible = readDataOverTimeChartContainer.style.display !== 'none';
                setToggleState(readCurrentYearChartContainer, readCurrentYearTableContainer, !chartVisible, readCurrentYearShowAsTableBtn);
                blurElementIfFocused(readShowCurrentDataBtn);
                moveFocusToElement(readShowDataOverTimeBtn);
            });
        }

        addKeyboardFocusTransfer(readShowDataOverTimeBtn, readShowCurrentDataBtn);
        addKeyboardFocusTransfer(readShowCurrentDataBtn, readShowDataOverTimeBtn);

        addEnterKeyHandler(readShowDataOverTimeBtn);
        addEnterKeyHandler(readShowCurrentDataBtn);
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

    function setToggleState(chartContainer, tableContainer, isChartVisible, btnShow) {
        chartContainer.style.display = isChartVisible ? 'none' : 'block';
        tableContainer.style.display = isChartVisible ? 'block' : 'none';

        var isTableVisible = tableContainer.style.display === 'block';
        setToggleText(btnShow, isTableVisible ? 'Show as a chart' : 'Show as a table')
        setAriaAttribute(btnShow, isChartVisible ? 'true' : 'false');
    }

})();