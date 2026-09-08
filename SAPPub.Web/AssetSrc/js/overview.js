document.addEventListener('DOMContentLoaded', () => {
    initialiseChartTableToggle('overview-english-maths-current-year');
    initialiseChartTableToggle('overview-destinations-current-year');
});

function initialiseChartTableToggle(prefix) {
    const button = document.getElementById(`${prefix}-show-btn`);
    const chartContainer = document.getElementById(`${prefix}-chart-container`);
    const tableContainer = document.getElementById(`${prefix}-table-container`);

    if (!button || !chartContainer || !tableContainer) {
        return;
    }

    button.setAttribute('aria-expanded', 'false');

    button.addEventListener('click', () => {
        const showingTable =
            tableContainer.style.display === 'block';

        if (showingTable) {
            tableContainer.style.display = 'none';
            chartContainer.style.display = 'block';

            button.textContent = 'Show as a table';
            button.setAttribute('aria-expanded', 'false');
        } else {
            chartContainer.style.display = 'none';
            tableContainer.style.display = 'block';

            button.textContent = 'Show as a chart';
            button.setAttribute('aria-expanded', 'true');
        }
    });
}