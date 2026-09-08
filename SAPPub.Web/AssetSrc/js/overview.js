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

    // JavaScript is available, so switch from the fallback table
    // to the enhanced chart view.
    button.classList.remove('govuk-!-display-none');
    chartContainer.classList.remove('govuk-!-display-none');
    tableContainer.classList.add('govuk-!-display-none');

    button.setAttribute('aria-expanded', 'false');

    button.addEventListener('click', () => {
        const tableIsVisible =
            !tableContainer.classList.contains('govuk-!-display-none');

        if (tableIsVisible) {
            // Table -> chart
            tableContainer.classList.add('govuk-!-display-none');
            chartContainer.classList.remove('govuk-!-display-none');

            button.textContent = 'Show as a table';
            button.setAttribute('aria-expanded', 'false');
        } else {
            // Chart -> table
            chartContainer.classList.add('govuk-!-display-none');
            tableContainer.classList.remove('govuk-!-display-none');

            button.textContent = 'Show as a chart';
            button.setAttribute('aria-expanded', 'true');
        }
    });
}