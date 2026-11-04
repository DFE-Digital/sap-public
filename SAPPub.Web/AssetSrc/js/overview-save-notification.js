(function () {
    function init() {
        // The save/remove success banner is rendered by the page after the form posts and reloads.
        // Its in the initial HTML so screen readers will not announce it as a live region.
        // When we move the focus to it, thats when it gets read out
        const banner = document.querySelector('[id$="-add-success"], [id$="-remove-success]');

        if (!banner) {
            return;
        }

        banner.scrollIntoView({ behavior: 'smooth', block: 'start' });

        banner.setAttribute('tabindex', '-1');
        banner.focus({ preventScroll: true });
    }

    document.addEventListener('DOMContentLoaded', init);
})();