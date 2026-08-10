(function () {
    document.addEventListener('DOMContentLoaded', () => {

        // all mes (meeting-exceeding-standards) current year related elements
        const allMesCurrentYearShowAsTableBtn = document.getElementById('all-mes-current-year-show-btn');
        const allMesCurrentYearChartContainer = document.getElementById('all-mes-current-year-chart-container');
        const allMesCurrentYearTableContainer = document.getElementById('all-mes-current-year-table-container');

        // all mes data over time related elements
        const allMesShowDataOverTimeBtn = document.getElementById('all-mes-show-data-over-time-btn');
        const allMesDataOverTimeChartContainer = document.getElementById('all-mes-data-over-time-chart-container');
        const allMesDataOverTimeTableContainer = document.getElementById('all-mes-data-over-time-table-container');
        const allMesDataOverTimeShowAsTableBtn = document.getElementById('all-mes-data-over-time-show-btn');

        const currentViewRadio = document.getElementById('current-view');
        const dataOvertimeViewRadio = document.getElementById('data-overtime-view');
        const allMesShowCurrentDataBtn = document.getElementById('all-mes-show-current-data-btn');


        setAriaAttribute(allMesCurrentYearShowAsTableBtn, 'false');
        if (allMesCurrentYearShowAsTableBtn) {
            allMesCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = allMesCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allMesCurrentYearChartContainer, allMesCurrentYearTableContainer, chartVisible, allMesCurrentYearShowAsTableBtn);
            });
        }

        if (allMesShowDataOverTimeBtn) {
            allMesShowDataOverTimeBtn.addEventListener('click', () => {
                if (dataOvertimeViewRadio) {
                    dataOvertimeViewRadio.checked = true;
                }

                var chartVisible = allMesCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allMesDataOverTimeChartContainer, allMesDataOverTimeTableContainer, !chartVisible, allMesDataOverTimeShowAsTableBtn);
                blurElementIfFocused(allMesShowDataOverTimeBtn);
                moveFocusToElement(allMesShowCurrentDataBtn);
            });
        }

        setAriaAttribute(allMesDataOverTimeShowAsTableBtn, 'false');
        if (allMesDataOverTimeShowAsTableBtn) {
            allMesDataOverTimeShowAsTableBtn.addEventListener('click', () => {
                const dataOverTimeChartVisible = allMesDataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(allMesDataOverTimeChartContainer, allMesDataOverTimeTableContainer, dataOverTimeChartVisible, allMesDataOverTimeShowAsTableBtn);
            });
        }

        if (allMesShowCurrentDataBtn) {
            allMesShowCurrentDataBtn.addEventListener('click', () => {
                if (currentViewRadio) {
                    currentViewRadio.checked = true;
                }

                var chartVisible = allMesDataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(allMesCurrentYearChartContainer, allMesCurrentYearTableContainer, !chartVisible, allMesCurrentYearShowAsTableBtn);
                blurElementIfFocused(allMesShowCurrentDataBtn);
                moveFocusToElement(allMesShowDataOverTimeBtn);
            });
        }

        addKeyboardFocusTransfer(allMesShowDataOverTimeBtn, allMesShowCurrentDataBtn);
        addKeyboardFocusTransfer(allMesShowCurrentDataBtn, allMesShowDataOverTimeBtn);

        addEnterKeyHandler(allMesShowDataOverTimeBtn);
        addEnterKeyHandler(allMesShowCurrentDataBtn);
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