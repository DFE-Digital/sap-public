(function () {
    document.addEventListener('DOMContentLoaded', () => {
        const breakdownGcseCurrentYearShowAsTableBtn = document.getElementById('breakdown-gcse-current-year-show-btn');
        const breakdownGcseCurrentYearChartContainer = document.getElementById('breakdown-gcse-current-year-chart-container');
        const breakdownGcseCurrentYearTableContainer = document.getElementById('breakdown-gcse-current-year-table-container');

        setAriaAttribute(breakdownGcseCurrentYearShowAsTableBtn, 'false');

        if (breakdownGcseCurrentYearShowAsTableBtn) {
            breakdownGcseCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = breakdownGcseCurrentYearChartContainer.style.display !== 'none';
                setToggleState(breakdownGcseCurrentYearChartContainer, breakdownGcseCurrentYearTableContainer, chartVisible, breakdownGcseCurrentYearShowAsTableBtn);
            });
        }
    });

    function setToggleState(chartContainer, tableContainer, isChartVisible, btnShow) {
        chartContainer.style.display = isChartVisible ? 'none' : 'block';
        tableContainer.style.display = isChartVisible ? 'block' : 'none';

        var isTableVisible = tableContainer.style.display === 'block';
        setToggleText(btnShow, isTableVisible ? 'Show as a chart' : 'Show as a table')
        setAriaAttribute(btnShow, isChartVisible ? 'true' : 'false');
    }

    function setToggleText(toggle, text) {
        if (toggle) toggle.textContent = text;
    }

    function setAriaAttribute(toggle, text) {
        if (toggle) toggle.setAttribute('aria-expanded', text);
    }
})();