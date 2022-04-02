using Notary.Api.DataAccess.Models;
using Notary.Api.DataAccess.Models.DTOs;


namespace Notary.Api.DataAccess.Interfaces
{
    public interface IComponentRepository
    {
        List<ComponentSDTO> GetComponents();

        Component GetComponentById(DynamicModelDTO model);

        Component CreateComponent(Component component);

        Component UpdateComponent(ComponentDTO model, int id);

        Component GetComponent(int id);

        Component GetComponentByIdentifier(ComponentIdDTO dto);

        void DeleteComponent(Component comp);

        
    }
}
