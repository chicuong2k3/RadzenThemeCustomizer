
window.setLoadingPercentage = (percentage) => {
    const container = document.querySelector('.loading-container');
    if (container) {
        container.style.setProperty('--blazor-load-percentage', percentage);
        container.style.setProperty('--blazor-load-percentage-text', `"${percentage}%"`);
        if (percentage >= 100) {
            container.style.opacity = '0';
            container.style.transition = 'opacity 2s ease';
            setTimeout(() => {
                container.style.display = 'none';
            }, 2000);
        } else {
            container.style.display = 'block';
            container.style.opacity = '1';
        }
    }
};

function StartBlazor() {
    let loadedCount = 0;
    let allResources = 0;

    Blazor.start({
        loadBootResource: function (type, filename, defaultUri, integrity) {
            if (type === "dotnetjs") {
                return defaultUri;
            }

            allResources++;
            const fetchResource = fetch(defaultUri, {
                cache: 'no-cache',
                integrity: integrity
            });

            fetchResource.then(() => {
                loadedCount++;
                const percentLoaded = Math.min(100, Math.round((loadedCount / allResources) * 100));
                window.setLoadingPercentage(percentLoaded);
            }).catch(() => {
                loadedCount++;
                const percentLoaded = Math.min(100, Math.round((loadedCount / allResources) * 100));
                window.setLoadingPercentage(percentLoaded);
            });

            return fetchResource;
        }
    });
}

document.addEventListener('DOMContentLoaded', () => {
    StartBlazor();
});