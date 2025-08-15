const messageForm = document.querySelector(".prompt__form");
const chatHistoryContainer = document.querySelector(".chats");
const suggestionItems = document.querySelectorAll(".suggests__item");

const themeToggleButton = document.getElementById("themeToggler");
const clearChatButton = document.getElementById("deleteButton");

let currentUserMessage = null;
let isGeneratingResponse = false;

// Create Chat Message Element
const createChatMessageElement = (htmlContent, ...cssClasses) => {
    const messageElement = document.createElement("div");
    messageElement.classList.add("message", ...cssClasses);
    messageElement.innerHTML = htmlContent;
    return messageElement;
};

// Typing Effect
const showTypingEffect = (rawText, htmlText, messageElement, incomingMessageElement, skipEffect = false) => {
    const copyIconElement = incomingMessageElement.querySelector(".message__icon");
    copyIconElement.classList.add("hide");

    if (skipEffect) {
        messageElement.innerHTML = htmlText;
        hljs.highlightAll();
        addCopyButtonToCodeBlocks();
        copyIconElement.classList.remove("hide");
        isGeneratingResponse = false;
        return;
    }

    const wordsArray = rawText.split(' ');
    let wordIndex = 0;

    const typingInterval = setInterval(() => {
        messageElement.innerText += (wordIndex === 0 ? '' : ' ') + wordsArray[wordIndex++];
        if (wordIndex === wordsArray.length) {
            clearInterval(typingInterval);
            isGeneratingResponse = false;
            messageElement.innerHTML = htmlText;
            hljs.highlightAll();
            addCopyButtonToCodeBlocks();
            copyIconElement.classList.remove("hide");
        }
    }, 75);
};

// Handle Outgoing Message
const handleOutgoingMessage = async () => {
    currentUserMessage = messageForm.querySelector(".prompt__form-input").value.trim() || currentUserMessage;
    if (!currentUserMessage || isGeneratingResponse) return;

    isGeneratingResponse = true;

    const outgoingMessageHtml = `
        <div class="message__content">
            <img class="message__avatar" src="/Assets/img/chat-icons/user.png" alt="User avatar">
            <p class="message__text">${currentUserMessage}</p>
        </div>
    `;
    const outgoingMessageElement = createChatMessageElement(outgoingMessageHtml, "message--outgoing");
    chatHistoryContainer.appendChild(outgoingMessageElement);

    messageForm.reset();
    document.body.classList.add("hide-header");

    const loadingHtml = `
        <div class="message__content">
            <img class="message__avatar" src="/Assets/img/chat-icons/terra.svg" alt="Terra avatar">
            <p class="message__text"></p>
            <div class="message__loading-indicator">
                <div class="message__loading-bar"></div>
                <div class="message__loading-bar"></div>
                <div class="message__loading-bar"></div>
            </div>
        </div>
        <span onClick="copyMessageToClipboard(this)" class="message__icon hide"><i class='bx bx-copy-alt'></i></span>
    `;
    const incomingMessageElement = createChatMessageElement(loadingHtml, "message--incoming", "message--loading");
    chatHistoryContainer.appendChild(incomingMessageElement);
    const messageTextElement = incomingMessageElement.querySelector(".message__text");

    try {
        const response = await fetch("/Chat/Respond", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ message: currentUserMessage })
        });

        if (!response.ok) throw new Error(`Server error: ${response.statusText}`);

        const data = await response.json();

        if (!data.success) {
            isGeneratingResponse = false;
            messageTextElement.innerText = data.error || "Unknown error";
            messageTextElement.closest(".message").classList.add("message--error");
            return;
        }

        const responseText = data.responseText || "No response";
        const parsedApiResponse = marked.parse(responseText);
        showTypingEffect(responseText, parsedApiResponse, messageTextElement, incomingMessageElement);
    } catch (error) {
        isGeneratingResponse = false;
        messageTextElement.innerText = error.message;
        messageTextElement.closest(".message").classList.add("message--error");
    } finally {
        incomingMessageElement.classList.remove("message--loading");
    }
};

// Add Copy Button to Code Blocks
const addCopyButtonToCodeBlocks = () => {
    const codeBlocks = document.querySelectorAll('pre');
    codeBlocks.forEach(block => {
        const codeElement = block.querySelector('code');
        let language = [...codeElement.classList].find(cls => cls.startsWith('language-'))?.replace('language-', '') || 'Text';

        const languageLabel = document.createElement('div');
        languageLabel.innerText = language.charAt(0).toUpperCase() + language.slice(1);
        languageLabel.classList.add('code__language-label');
        block.appendChild(languageLabel);

        const copyButton = document.createElement('button');
        copyButton.innerHTML = `<i class='bx bx-copy'></i>`;
        copyButton.classList.add('code__copy-btn');
        block.appendChild(copyButton);

        copyButton.addEventListener('click', () => {
            navigator.clipboard.writeText(codeElement.innerText).then(() => {
                copyButton.innerHTML = `<i class='bx bx-check'></i>`;
                setTimeout(() => copyButton.innerHTML = `<i class='bx bx-copy'></i>`, 2000);
            }).catch(err => {
                console.error("Copy failed:", err);
                alert("Unable to copy text!");
            });
        });
    });
};

// Copy Message to Clipboard
const copyMessageToClipboard = (copyButton) => {
    const messageContent = copyButton.parentElement.querySelector(".message__text").innerText;
    navigator.clipboard.writeText(messageContent);
    copyButton.innerHTML = `<i class='bx bx-check'></i>`;
    setTimeout(() => copyButton.innerHTML = `<i class='bx bx-copy-alt'></i>`, 1000);
};

// Delete Chat History
const deleteForm = document.getElementById("deleteChatForm");
deleteForm.addEventListener("submit", e => {
    if (!confirm("Are you sure you want to delete all chat history?")) {
        e.preventDefault();
    } else {
        chatHistoryContainer.innerHTML = '';
        currentUserMessage = null;
        isGeneratingResponse = false;
        document.body.classList.remove("hide-header");
    }
});

// Suggestion Item Click
suggestionItems.forEach(suggestion => {
    suggestion.addEventListener('click', () => {
        currentUserMessage = suggestion.querySelector(".suggests__item-text").innerText;
        handleOutgoingMessage();
    });
});

// Form Submit Send Message
messageForm.addEventListener('submit', e => {
    e.preventDefault();
    handleOutgoingMessage();
});

// Formats Numbered Lists
document.addEventListener("DOMContentLoaded", () => {
    const messageElements = document.querySelectorAll(".message__text");

    messageElements.forEach(el => {
        let rawText = el.textContent || "";

        rawText = rawText.replace(/\r\n/g, '\n').replace(/\r/g, '\n');

        const escapeHtml = (text) =>
            text.replace(/&/g, "&amp;")
                .replace(/</g, "&lt;")
                .replace(/>/g, "&gt;")
                .replace(/"/g, "&quot;")
                .replace(/'/g, "&#039;");

        rawText = escapeHtml(rawText);
        rawText = rawText.replace(/\*\*/g, '');

        const lines = rawText.split('\n');

        let html = "";
        let previousWasNumbered = false;

        for (let i = 0; i < lines.length; i++) {
            const line = lines[i];
            const trimmed = line.trim();

            const isNumbered = /^\d+\.\s/.test(trimmed);
            const isBullet = /^[ \t]*([•*\-\+→»►])\s/.test(trimmed);

            if (isNumbered) {
                let formattedLine = line.replace(/^[ \t]*(\d+\.\s)([^:]+)(:)/,
                    (match, p1, p2, p3) =>
                        p1 + `<span style="font-weight:bold;">${p2}</span>` + p3);
                formattedLine = formattedLine.replace(/^(\d+\.)\s/,
                    (match, p1) =>
                        `<span style="display:inline-block; width:30px; text-align:right; margin-right:8px; vertical-align:top;">${p1}</span> `);

                html += `<p style="padding-left: 60px; text-indent: -38px; margin: 0 0 1em 0; white-space: pre-wrap;">${formattedLine.trim()}</p>`;
                previousWasNumbered = true;
            } else if (isBullet) {
                let formattedLine = line.replace(/^[ \t]*([•*\-\+→»►])\s/,
                    (match, p1) =>
                        `<span style="display:inline-block; width:30px; text-align:right; margin-right:8px; vertical-align:top;">${p1}</span> `);

                html += `<p style="padding-left: 60px; text-indent: -38px; margin: 0 0 1em 0; white-space: pre-wrap;">${formattedLine.trim()}</p>`;
                previousWasNumbered = false;
            } else {
                html += `<div style="margin: 0 0 1em 0; padding-left: 0; text-indent: 0; white-space: normal;"><p>${line.trim()}</p></div>`;
                previousWasNumbered = false;
            }
        }

        el.innerHTML = html;
    });
});