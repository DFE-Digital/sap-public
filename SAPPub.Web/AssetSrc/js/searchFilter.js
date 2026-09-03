(function () {
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initFilterProtection);
    } else {
        initFilterProtection();
    }

    function initFilterProtection() {
        const filterForm = document.getElementById('app-filter-panel');
        if (!filterForm) return;

        const applyButtons = document.getElementsByClassName('app-filter__apply-button');

        filterForm.onsubmit = function (e) {
            const submitEvent = e;
            const submitter = submitEvent.submitter;
            const isApplyButton = [...applyButtons].indexOf(submitter) > -1;

            if (!isApplyButton) {
                e.preventDefault();
                return false;
            }

            for (var applyButton of applyButtons) {
                applyButton.classList.add('govuk-button--loading');
                applyButton.disabled = true;
            }
        };

        const checkboxes = filterForm.querySelectorAll('input[type="checkbox"]');
        checkboxes.forEach(checkbox => {
            const newCheckbox = checkbox.cloneNode(true);
            checkbox.parentNode.replaceChild(newCheckbox, checkbox);
        });
    }
})();

(function () {
    const sections = document.querySelectorAll('[data-module="app-filter-section"]');
    sections.forEach(section => {
        const toggle = section.querySelector('.app-filter-section__toggle');
        const content = section.querySelector('.app-filter-section__content');

        if (toggle && content) {
            toggle.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                const isExpanded = toggle.getAttribute('aria-expanded') === 'true';
                toggle.setAttribute('aria-expanded', !isExpanded);

                if (isExpanded) {
                    content.setAttribute('hidden', '');
                } else {
                    content.removeAttribute('hidden');
                }
            });
        }
    });

    const filterToggle = document.querySelector('[data-module="app-filter-toggle"]');
    const filterPanel = document.getElementById('app-filter-panel');

    if (filterToggle && filterPanel) {
        filterToggle.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            const isExpanded = filterToggle.getAttribute('aria-expanded') === 'true';
            filterToggle.setAttribute('aria-expanded', !isExpanded);

            if (isExpanded) {
                filterPanel.classList.remove('app-filter-panel--visible');
            } else {
                filterPanel.classList.add('app-filter-panel--visible');
            }
        });
    }
})();