(function () {
    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll(".chart-table-toggle").forEach(initChartTableToggle);
    });

    function initChartTableToggle(root) {
        const idPrefix = root.dataset.idPrefix;
        if (!idPrefix) {
            return;
        }

        // current year related elements
        const currentYearShowAsTableBtn = root.querySelector(`#${idPrefix}-current-year-show-btn`);
        const currentYearChartContainer = root.querySelector(`#${idPrefix}-current-year-chart-container`);
        const currentYearTableContainer = root.querySelector(`#${idPrefix}-current-year-table-container`);

        // data over time related elements
        const showDataOverTimeBtn = root.querySelector(`#${idPrefix}-show-data-over-time-show-btn`);
        const dataOverTimeChartContainer = root.querySelector(`#${idPrefix}-data-over-time-chart-container`);
        const dataOverTimeTableContainer = root.querySelector(`#${idPrefix}-data-over-time-table-container`);
        const dataOverTimeShowAsTableBtn = root.querySelector(`#${idPrefix}-current-year-show-btn`);

        const currentViewRadio = root.querySelector(`#${idPrefix}-current-view`);
        const dataOvertimeViewRadio = root.querySelector(`#${idPrefix}-data-overtime-view`);
        const showCurrentDataBtn = root.querySelector(`#${idPrefix}-show-current-data-btn`);


        setAriaAttribute(currentYearShowAsTableBtn, 'false');
        if (currentYearShowAsTableBtn) {
            currentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = currentYearChartContainer.style.display !== 'none';
                setTooggleState(currentYearChartContainer, currentYearTableContainer, chartVisible, currentYearShowAsTableBtn);
            });
        }

        if (showDataOverTimeBtn) {
            showDataOverTimeBtn.addEventListener('click', () => {
                if (dataOvertimeViewRadio) {
                    dataOvertimeViewRadio.checked = true;
                }

                var chartVisible = currentYearChartContainer.style.display !== 'none';
                setTooggleState(dataOverTimeChartContainer, dataOverTimeTableContainer, !chartVisible, dataOverTimeShowAsTableBtn);
                blurElementIfFocused(showDataOverTimeBtn);
                moveFocusToElement(showCurrentDataBtn);
            });
        }

        setAriaAttribute(dataOverTimeShowAsTableBtn, 'false');
        if (dataOverTimeShowAsTableBtn) {
            dataOverTimeShowAsTableBtn.addEventListener('click', () => {
                const dataOverTimeChartVisible = dataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(dataOverTimeChartContainer, dataOverTimeTableContainer, dataOverTimeChartVisible, dataOverTimeShowAsTableBtn);
            });
        }

        if (showCurrentDataBtn) {
            showCurrentDataBtn.addEventListener('click', () => {
                if (currentViewRadio) {
                    currentViewRadio.checked = true;
                }

                var chartVisible = dataOverTimeChartContainer.style.display !== 'none';
                setTooggleState(currentYearChartContainer, currentYearTableContainer, !chartVisible, currentYearShowAsTableBtn);
                blurElementIfFocused(showCurrentDataBtn);
                moveFocusToElement(showDataOverTimeBtn);
            });
        }

        addKeyboardFocusTransfer(showDataOverTimeBtn, showCurrentDataBtn);
        addKeyboardFocusTransfer(showCurrentDataBtn, showDataOverTimeBtn);

        addEnterKeyHandler(showDataOverTimeBtn);
        addEnterKeyHandler(showCurrentDataBtn);
    }

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