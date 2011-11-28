using System;
using DDDSample.Application.Commands;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.DomainModel.Potential.Location;
using LeanCommandUnframework;
using HandlingEvent = DDDSample.DomainModel.Operations.Handling.HandlingEvent;

namespace DDDSample.Application.CommandHandlers
{
    public class RegisterHandlingEventCommandHandler : IHandler<RegisterHandlingEventCommand>
    {
        private readonly IHandlingEventRepository _handlingEventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICargoRepository _cargoRepository;

        public RegisterHandlingEventCommandHandler(IHandlingEventRepository handlingEventRepository, ILocationRepository locationRepository, ICargoRepository cargoRepository)
        {
            _handlingEventRepository = handlingEventRepository;
            _cargoRepository = cargoRepository;
            _locationRepository = locationRepository;
        }

        public object Handle(RegisterHandlingEventCommand command)
        {
            var trackingId = new TrackingId(command.TrackingId);
            var cargo = _cargoRepository.Find(trackingId);
            var occuranceLocationUnLocode = new UnLocode(command.OccuranceLocation);
            var occuranceLocation = _locationRepository.Find(occuranceLocationUnLocode);
            var evnt = new HandlingEvent(command.Type, occuranceLocation, DateTime.Now, command.CompletionTime, cargo);
            _handlingEventRepository.Store(evnt);

            return null;
        }
    }
}