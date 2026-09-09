document.addEventListener('DOMContentLoaded', () => {
    const button =
        document.getElementById(
            'overview-destinations-current-year-show-btn');

    const chartContainer =
        document.getElementById(
            'overview-destinations-current-year-chart-container');

    const tableContainer =
        document.getElementById(
            'overview-destinations-current-year-table-container');

    if (!button || !chartContainer || !tableContainer) {
        return;
    }

    button.setAttribute('aria-expanded', 'false');

    button.addEventListener('click', () => {
        const chartVisible =
            chartContainer.style.display !== 'none';

        chartContainer.style.display =
            chartVisible ? 'none' : 'block';

        tableContainer.style.display =
            chartVisible ? 'block' : 'none';

        button.textContent =
            chartVisible
                ? 'Show as a chart'
                : 'Show as a table';

        button.setAttribute(
            'aria-expanded',
            chartVisible ? 'true' : 'false');
    });
});