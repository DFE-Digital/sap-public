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
                setTooggleState(allGcseDataOverTimeChartContainer, allGcseDataOverTimeTableContainer, false, allGcseCurrentYearShowAsTableBtn);
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
                setTooggleState(allGcseCurrentYearChartContainer, allGcseCurrentYearTableContainer, false, allGcseCurrentYearShowAsTableBtn);
            });
        }

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

    function setTooggleState(chartContainer, tableContainer, isChartVisible, btnShow) {
        chartContainer.style.display = isChartVisible ? 'none' : 'block';
        tableContainer.style.display = isChartVisible ? 'block' : 'none';

        setToggleText(btnShow, isChartVisible ? 'Show as a chart' : 'Show as a table')
        setAriaAttribute(btnShow, isChartVisible ? 'true' : 'false');
    }

})();