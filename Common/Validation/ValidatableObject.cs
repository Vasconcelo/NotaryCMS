namespace Common.Validation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using FluentValidation;

    /// <summary>
    /// Represents an object that is validated.
    /// </summary>
    /// <typeparam name="TValidator">
    /// The validator used to validate the object.
    /// </typeparam>
    public abstract class ValidatableObject<TValidator> : IValidatableObject where TValidator : IValidator, new()
    {
        private readonly IValidator _validator;

        /// <summary>
        /// Initializes a new instance of the NNA.Services.Common.Validation.ValidatableObject class.
        /// </summary>
        protected ValidatableObject()
        {
            _validator = new TValidator();
        }

        /// <summary>
        /// Validate an object.
        /// </summary>
        /// <param name="validationContext">
        /// The validation context.
        /// </param>
        /// <returns>
        /// The validation results.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return _validator.Validate((IValidationContext)this).Errors.Select(item => new ValidationResult(item.ErrorMessage, new List<string> { item.PropertyName }));
        }

        /// <summary>
        /// Return a serialized JSON representation of the object. A maximum of 2048 characters are returned.
        /// </summary>
        /// <returns>
        /// A serialized JSON representation of the object.
        /// </returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this).PadLeft(2048);
        }
    }
}
