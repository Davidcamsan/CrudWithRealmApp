using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CrudWithRealmApp.Models;
using System.Collections.Generic;
using Realms;
using System.Linq;
using Xamarin.Forms;
using CrudWithRealmApp.Views;

namespace CrudWithRealmApp.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        
        public CustomerViewModel()
        {
            ListOfCustomerDetails = _getRealmInstance.All<CustomerDetails>().ToList();
        }


        //List of Customers
        private List<CustomerDetails> _listOfCustomerDetails = new List<CustomerDetails>();

        public List<CustomerDetails> ListOfCustomerDetails
        {
            get { return _listOfCustomerDetails; }

            set
            {
                _listOfCustomerDetails = value;
                OnPropertyChanged();
            }
        }





        //Customer Instance
        private CustomerDetails _customerDetails = new CustomerDetails();

        public CustomerDetails CustomerDetails
        {
            get { return _customerDetails; }

            set 
            {
                _customerDetails = value;
                OnPropertyChanged();
            }
        }

        //COMMANDS

        //CREATE_COMMAND
        public Command CreateCommand //Adds Customer to Realm Database
        {
            get
            {
                return new Command(() =>
                {

                    _customerDetails.CustomerId = _getRealmInstance.All<CustomerDetails>().Count() + 1;
                    _getRealmInstance.Write(() =>
                    {
                        _getRealmInstance.Add(_customerDetails);
                    });
                });
            }
        }


        //UPDATE_COMMAND
        public Command UpdateCommand
        {
            get
            {
                return new Command(() =>
                {
                    var customerDetailsUpdate = new CustomerDetails //We create a temp instance with the new data.
                    {
                        CustomerId = _customerDetails.CustomerId,
                        CustomerName = _customerDetails.CustomerName,
                        CustomerAge = _customerDetails.CustomerAge
                    };


                    _getRealmInstance.Write(() =>
                    {
                        _getRealmInstance.Add(customerDetailsUpdate, update: true); //we pass temp instance, it updates stored Customer that matches temp's ID
                    });
                });
            }
        }



        //REMOVE_COMMAND
        public Command RemoveCommand
        {
            get
            {
                return new Command(() =>
                {
                    var getAllCustomerDetailsById = _getRealmInstance.All<CustomerDetails>().First(x => x.CustomerId == _customerDetails.CustomerId);

                    using (var transaction = _getRealmInstance.BeginWrite())
                    {
                        _getRealmInstance.Remove(getAllCustomerDetailsById);
                        transaction.Commit();
                    };
                });
            }
        }



        //PAGE NAVIGATION COMMANDS
        public Command NavToListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new ListOfCustomers());
                });
            }
        }

        public Command NavToCreateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new CreateCustomer());
                });
            }
        }

        public Command NavToUpdateDeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new UpdateOrDeleteCustomer());
                });
            }
        }





        //Realm Stuff
        Realm _getRealmInstance = Realm.GetInstance();



        //I NOTIFY STUFF
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
