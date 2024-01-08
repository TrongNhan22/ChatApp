using ChatApp.Models;
using ChatApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ChatApp.ViewModels;
using ChatApp.Interface;

namespace ChatApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageService)
        {
            _messageRepository = messageService;
        }
        public async Task<IActionResult> Index()
        {
            return View(new MessageViewModel { });
        }
        [HttpGet]
        public async Task<IActionResult>Index(string receiverId = "6534e66f77ca367b832706e6")
        {
            //For testing
            string senderId = "65340b20b32df212d36f15ad";
            var messages = await _messageRepository.GetListBySenderAndReceiverIdAsync(senderId, receiverId);
            var chatPartnerList = await _messageRepository.GetChatPartner();
            var messageViewModel = new MessageViewModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Messages = messages,
                ChatPartners=chatPartnerList
            };
            return View(messageViewModel);

        }
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get()
        {
            string senderId = "65340b20b32df212d36f15ad"; string receiverId = "6534e66f77ca367b832706e6";
            var messages = await _messageRepository.GetListBySenderAndReceiverIdAsync(senderId, receiverId);
            if (messages is null) return NotFound();
            var messageViewModel = new MessageViewModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Messages = messages
            };
            return View();
            return View("Index", messageViewModel);
        }
        //public async Task<IActionResult> Post(MessageViewModel messageViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var message = new Message
        //        { 
        //            SenderId = messageViewModel.SenderId,
        //            ReceiverId = messageViewModel.ReceiverId,
        //            Content = messageViewModel.Content,
        //            Media=messageViewModel.Media,
        //            Date = DateTime.Now.ToString(),
        //        };
        //        await _messageRepository.CreateAsync(message);
        //    }
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return View();
        //    //}
        //    //return RedirectToAction("Index");
        //    //_messageService.CreateAsync(newMessage);
        //    //await _messageService.CreateAsync(newMessage);
        //    //return CreatedAtAction(nameof(Get), new { id = newMessage.Id }, newMessage);

        //}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Message updatedMessage)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message is null) return NotFound();
            updatedMessage.Id = message.Id;
            await _messageRepository.UpdateAsync(id, updatedMessage);
            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message is null) return NotFound();
            await _messageRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}