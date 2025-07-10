window.injectCss = function (css) {
    let styleElement = document.getElementById('dynamic-theme');
    if (!styleElement) {
        styleElement = document.createElement('style');
        styleElement.id = 'dynamic-theme';
        document.head.appendChild(styleElement);
    }
    styleElement.textContent = css;
};
