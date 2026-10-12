(function () {
    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll('.chart-table-toggle').forEach(initChartTableToggle);
    });

    function initChartTableToggle(root) {
        initChartTableButtons(root);

        // Show data over time/Show current data are links that reload the page with the chosen view
        // so they work without javascript. With javascript, switch the view in place instead:
        root.querySelectorAll("[data-show-view]").forEach(link => {
            link.addEventListener('click', (event) => {
                event.preventDefault();
                showView(root, link.dataset.showView);
            });
        });
    }



    function initChartTableButtons(root) {
        const charts = root.querySelectorAll('.chart-table-toggle__chart');
        const tables = root.querySelectorAll('.chart-table-toggle__table');
        const buttons = root.querySelectorAll('.chart-table-toggle__table-btn');
        let isTableVisible = false;

        buttons.forEach(button => {
            button.setAttribute('aria-expanded', 'false');

            button.addEventListener('click', () => {
                isTableVisible = !isTableVisible;

                charts.forEach(chart => chart.style.display = isTableVisible ? 'none' : 'block');
                tables.forEach(table => table.style.display = isTableVisible ? 'block' : 'none');
                buttons.forEach(btn => {
                    btn.textContent = isTableVisible ? 'Show as a chart' : 'Show as a table';
                    btn.setAttribute('aria-expanded', String(isTableVisible));
                });
            });
        });
    }

    function showView(root, view) {
        root.dataset.view = view;

        root.querySelectorAll('[data-view-panel]').forEach(panel => {
            panel.hidden = panel.dataset.viewPanel !== view;
        });

        const switchBackLink = root.querySelector(`[data-view-panel="${view}"] [data-show-view]`);
        if (switchBackLink) {
            switchBackLink.focus();
        }
    }
})();