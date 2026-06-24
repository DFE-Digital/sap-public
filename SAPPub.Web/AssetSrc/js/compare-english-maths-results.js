(function () {
    document.addEventListener('DOMContentLoaded', () => {

        // all gcse current year related elements
        const allGcseCurrentYearShowAsTableBtn = document.getElementById('all-gcse-current-year-show-btn');
        const allGcseCurrentYearChartContainer = document.getElementById('all-gcse-current-year-chart-container');
        const allGcseCurrentYearTableContainer = document.getElementById('all-gcse-current-year-table-container');

        setAriaAttribute(allGcseCurrentYearShowAsTableBtn, 'false');

        if (allGcseCurrentYearShowAsTableBtn) {
            allGcseCurrentYearShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = allGcseCurrentYearChartContainer.style.display !== 'none';
                setTooggleState(allGcseCurrentYearChartContainer, allGcseCurrentYearTableContainer, chartVisible, allGcseCurrentYearShowAsTableBtn);
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