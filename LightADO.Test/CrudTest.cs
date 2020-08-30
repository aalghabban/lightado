using Xunit;

namespace LightADO.Test
{
    public class CrudTest
    {
        [Fact]
        public void Create()
        {
            Company newCompany = new Company();
            newCompany.Name = "test";
            newCompany.IsActive = true;
            newCompany.Create();
            Assert.True(newCompany.ID > 0);
        }
        
        [Fact]
        public void GetById()
        {
            Company x = new Company(1);
            Assert.True(new Company(1).ID > 0);
        }
    }
    
    [Table(Name="Companies")]
    public class Company : CrudBase<Company>
    {
        public Company()
        {
            
        }

        public Company(int id) : base(id)
        {
        }

        [PrimaryKey]
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
    }
}