(function () {
    function init() {
        const notification =
            document.querySelector('.overview-save-notification');

        if (!notification) {
            return;
        }

        notification.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });

        const banner =
            notification.querySelector('.govuk-notification-banner');

        if (banner) {
            banner.setAttribute('tabindex', '-1');
            banner.focus({
                preventScroll: true
            });
        }
    }

    document.addEventListener(
        'DOMContentLoaded',
        init);
})();