using Notary.CMS.DataAccess.Models.DTOs;
using Notary.CMS.DataAccess.Models;


namespace Notary.CMS.DataAccess.Interfaces
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
