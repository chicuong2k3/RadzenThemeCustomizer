window.downloadFile = async function (url, data) {
    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });

    if (!response.ok) {
        return;
    }

    console.log(response);

    const blob = await response.blob();

    const contentDisposition = response.headers.get("Content-Disposition") || "";
    let filename = "downloaded_file";

    const utf8Match = contentDisposition.match(/filename\*\=UTF-8''([^;]+)/);
    const asciiMatch = contentDisposition.match(/filename="?([^\";]+)"?/);

    if (utf8Match) {
        filename = decodeURIComponent(utf8Match[1]);
    } else if (asciiMatch) {
        filename = asciiMatch[1];
    }

    const downloadUrl = window.URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = downloadUrl;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(downloadUrl);
}
