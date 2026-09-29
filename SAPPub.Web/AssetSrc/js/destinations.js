(function () {
    document.addEventListener('DOMContentLoaded', () => {
        // KS5 destinations (edu, apprnship, work)
        const allKs5DestsShowAsTableBtn = document.getElementById('all-ks5-dest-data-show-btn');
        const allKs5DestChartContainer = document.getElementById('all-ks5-dest-data-chart-container');
        const allKs5DestTableContainer = document.getElementById('all-ks5-dest-data-table-container');

        setAriaAttribute(allKs5DestsShowAsTableBtn, 'false');
        if (allKs5DestsShowAsTableBtn) {
            allKs5DestsShowAsTableBtn.addEventListener('click', () => {
                const chartVisible = allKs5DestChartContainer.style.display !== 'none';
                setToggleState(allKs5DestChartContainer, allKs5DestTableContainer, chartVisible, allKs5DestsShowAsTableBtn);
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

    function setToggleState(chartContainer, tableContainer, isChartVisible, btnShow) {
        chartContainer.style.display = isChartVisible ? 'none' : 'block';
        tableContainer.style.display = isChartVisible ? 'block' : 'none';

        var isTableVisible = tableContainer.style.display === 'block';
        setToggleText(btnShow, isTableVisible ? 'Show as a chart' : 'Show as a table')
        setAriaAttribute(btnShow, isChartVisible ? 'true' : 'false');
    }
})();