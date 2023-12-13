function createChatBubble(message, name) {
    return (message.senderId == "SYSTEM")
        ?
        `
                <div class="d-flex flex-row justify-content-center" >
                    <div>
                        <p class="small p-2 ms-3 mb-1 rounded-3" style = "background-color: #a3ffb4;" >
                            ${message.content}
                        </p>
             
                    </div>
                </div>
        `
        : (message.senderId === name)
            ?
            `
                    <div class="d-flex flex-row justify-content-end" >
                        <div>
                            <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" >
                                ${message.content}
                            </p>
                            <p class="small me-3 mb-3 rounded-3 text-muted"> 12: 00 PM | Aug 13 </p>
            `
            :
            `
                    <div class="d-flex flex-row justify-content-start" >
                        <p>${message.senderId} </p>
                        <div>
                            <p class="small p-2 ms-3 mb-1 rounded-3" style = "background-color: #f5f6f7;" >
                                ${message.content}
                            </p>
                            <p class="small ms-3 mb-3 rounded-3 text-muted float-end">
                                12: 00 PM | Aug 13
                            </p>
                        </div>
                    </div>
            `;
}