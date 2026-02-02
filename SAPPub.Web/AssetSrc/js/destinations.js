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

        const allDestShowCurrentDataBtn = document.getElementById('all-dest-show-current-data-btn');

        setAriaAttribute(allDestCurrentYearShowAsTableBtn, 'false');
        allDestCurrentYearShowAsTableBtn.addEventListener('click', () => {
            const chartVisible = allDestCurrentYearChartContainer.style.display !== 'none';
            setTooggleState(allDestCurrentYearChartContainer, allDestCurrentYearTableContainer, chartVisible, allDestCurrentYearShowAsTableBtn);
        });

        setAriaAttribute(allDestShowDataOverTimeBtn, 'false');
        allDestShowDataOverTimeBtn.addEventListener('click', () => {
            setTooggleState(allDestDataOverTimeChartContainer, allDestDataOverTimeTableContainer, false, allDestCurrentYearShowAsTableBtn);
            setAriaAttribute(allDestShowDataOverTimeBtn, 'true');
        });

        setAriaAttribute(allDestDataOverTimeShowAsTableBtn, 'false');
        allDestDataOverTimeShowAsTableBtn.addEventListener('click', () => {
            const dataOverTimeChartVisible = allDestDataOverTimeChartContainer.style.display !== 'none';
            setTooggleState(allDestDataOverTimeChartContainer, allDestDataOverTimeTableContainer, dataOverTimeChartVisible, allDestDataOverTimeShowAsTableBtn);
        });

        allDestShowCurrentDataBtn.addEventListener('click', () => {
            setTooggleState(allDestCurrentYearChartContainer, allDestCurrentYearTableContainer, false, allDestCurrentYearShowAsTableBtn);
        });
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