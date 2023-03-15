﻿using ECommerceBE.Application.Abstraction.Services;
using MediatR;

namespace ECommerceBE.Application.Features.Commands.Basket.UpdateQuantity
{
    public class UpdateQuantityCommandHandler : IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>
    {
        readonly IBasketService _basketService;

        public UpdateQuantityCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<UpdateQuantityCommandResponse> Handle(UpdateQuantityCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.UpdateQuantityAsync(new()
            {
                Quantity = request.Quantity,
                BasketItemId= request.BasketItemId,
            });
            return new();
        }
    }
}
