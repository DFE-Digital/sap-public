(function () {
    document.addEventListener('DOMContentLoaded', () => {

        // all destinations current year related elements
        const allDestCurrentYearShowAsTableBtn = document.getElementById('all-dest-current-year-show-btn');
        const allDestCurrentYearChartContainer = document.getElementById('all-dest-current-year-chart-container');
        const allDestCurrentYearTableContainer = document.getElementById('all-dest-current-year-table-container');


        //const allDestShowCurrentDataBtn = document.getElementById('all-dest-show-current-data-btn');

        setAriaAttribute(allDestCurrentYearShowAsTableBtn, 'false');
        if (allDestCurrentYearShowAsTableBtn) {
            allDestCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = allDestCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allDestCurrentYearChartContainer, allDestCurrentYearTableContainer, chartVisible, allDestCurrentYearShowAsTableBtn);
            });
        }

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

    function setTooggleState(chartContainer, tableContainer, isChartVisible, btnShow) {
        chartContainer.style.display = isChartVisible ? 'none' : 'block';
        tableContainer.style.display = isChartVisible ? 'block' : 'none';

        var isTableVisible = tableContainer.style.display === 'block';
        setToggleText(btnShow, isTableVisible ? 'Show as a chart' : 'Show as a table')
        setAriaAttribute(btnShow, isChartVisible ? 'true' : 'false');
    }

})();